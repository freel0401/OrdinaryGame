_G.CONFPATH=[[conf.lua]]
dofile'lib/log.lua'
dofile'lib/dump.lua'
dofile'lib/table.lua'
dofile'lib/time.lua'
dofile'lib/string.lua'
dofile'lib/assert.lua'
dofile'lib/object.lua'
dofile'lib/bit.lua'
dofile'lib/createtable.lua'
dofile'lib/leak.lua'
dofile'lib/ip.lua'
dofile'lib/debug.lua'
dofile'lib/utility.lua'
dofile'lib/guid.lua'
local opent = false
local _tkey = '_T'
local print = print0 or Log.sys
local zipdatas, loadfeast = {}, false

--验证数据
local function check( row, col )
	if tonumber(row) == nil or row < 1 then error("Row is not a valid number!") end
	if tonumber(col) == nil or col < 1 then error("Column is not a valid number!") end
end

--返回excel形式的坐标
local function excelCoord( row, col )
	check(1, col)
	local c=''
	local m=0
	while true do
		m = col%26
		if m == 0 then
			c = 'Z'..c
			col = col - 26
		else
			c = string.char((string.byte('A')-1+m))..c
		end
		col = (col - m)/26
		if col == 0 then
			break
		end
	end
	return row and c..row or c
end

--- 打开/关闭一个全局的Excel进程
local globalExcel
local function ExcelOpen()
	globalExcel=globalExcel or luacom.CreateObject("Excel.Application")
	if globalExcel == nil then error("Object is not create") end
end
local function ExcelClose()
	globalExcel:Quit()
	globalExcel=nil
	collectgarbage('collect')
end

--- 打开已有的Excel文件
local function ExcelBookOpen(filePath, visible,readOnly)
	local oExcel = globalExcel
	if oExcel == nil then error("Object is not create") end
	--查看文件是否存在
--	local t=io.open(filePath,"r")
--	if t == nil then
	--文件不存在时的处理
--		oExcel.Application:quit()
--		oExcel=nil
--		error(filePath.." File does not exist")
--	else
--		t:close()
--	end
	oExcel.Visible = visible
	local oWorkBook = oExcel.WorkBooks:Open(filePath, nil)
	print("!!!!!!! kk", oWorkBook.Application.Calculation,  oWorkBook.Application.CalculateBeforeSave )
	print("!!! cc", oWorkBook.Application:Calculate() )
	return oExcel,oWorkBook
end

-- 关闭Excel Workbook
local function ExcelWorkBookClose(oWorkBook)
	if oWorkBook == nil then error("workbook is nil!") end
	oWorkBook:Close( false )
	print("!!!! work book close")
end

--关闭Excel文件
local function ExcelBookClose(oExcel,save,alert)
	if oExcel == nil then error("oExcel is not a object!") end
	if save then oExcel.ActiveWorkBook:save() end
	print("!!!!!! Excel close")
	oExcel.Application.DisplayAlerts = false
	oExcel.Application.ScreenUpdating = alert and 1 or 0
	oExcel.Application:Close()
	return true
end

--列出所有Sheet
local function ExcelSheetList(oExcel)
	if oExcel == nil then error("oExcel is not a object!") end
	local tab = {}
	local count = oExcel.ActiveWorkbook.Sheets.Count
	for i = 1,count do
		tab[i] = oExcel.ActiveWorkbook.Sheets(i).Name
	end
	return tab
end

local sheetcache -- 为了优化读取速度，在active的时候预读取所有数据

--- 使用key字段作为主键建立新表
local function indexPK(tb, key)
	local s = {}
	for k, v in pairs(tb) do
		local vk = v[key]
		if vk then
			if s[vk] then
				error("duplicate key value "..vk)
			end
			s[vk] = v
		else
			error (key.." does not exists in "..k)
		end
	end
	return s
end

--- 空串检查
local function emptyStr(value)
	-- 空字符串检查
	if value and type(value)=='string' and string.find(value, "^%s+$") then
		return value, '!!!WARNING: Empty string', 1 -- level 1 warning
	end

	if value and type(value)=='string' then
		assert( not string.find(value, "^%s+"), 'space in front:'..value )
		assert( not string.find(value, "%s+$"), 'space in end:'..value )
	end

	-- -- 开头空串检查
	-- if value and type(value)=='string' and string.find(value, "^%s+") then
	-- 	return value, '!!!WARNING: Empty string at start', 1
	-- end

	-- -- 结尾空串检查
	-- if value and type(value)=='string' and string.find(value, "%s+$") then
	-- 	return value, '!!!WARNING: Empty string start in the end', 1
	-- end

	return value==nil and '' or type(value)=='number' and ('%.12g'):format(value) or tostring(value)
end

--- 把str字符串转化为tp指定的类型，并检查；null指示是否可以为空,default指示是否为空填充默认值。如果失败，返回nil,'出错信息',出错级别；否则只返回一个值
local function str2type(str, tp, null, default, fork, colname)
	if str == nil then
		if null == true then
			if default == true then
				if tp == 'i' or tp == 'f' then return 0 end
				if tp == 's' then return '' end
				if tp == 'b' then return false end
				return nil
			else
				return nil
			end
		else
			return nil, '!!!Error: Nil value is not permitted', 2 -- level 2 error
		end
	end

	if tp == 'i' then -- 整数
		if string.find(str, '^%s*[%+%-]?%d+%s*$') or string.find(str, '^%s*[%+%-]?0x[a-fA-F0-9]+%s*$')then
			return tonumber(str)
		else
			return nil, '!!!ERROR: It is not integer', 2
		end
	elseif tp == 'f' then -- 浮点数
		if string.find(str, '^%s*[%+%-]?%d*%.?%d+%s*$')then
			return tonumber(str)
		else
			return nil, '!!!ERROR: It is not float', 2
		end
	elseif tp == 's' then -- 字符串
		str = emptyStr(str)
		if fork and str:find'[\128-\255]' then
			forkText(str, colname)
		end
		return str
	elseif tp == 'b' then -- 布尔
		if str == 'true' or str == 'TRUE' or str == 1 or str == true or str == 'y' then
			return true
		elseif str == 'false' or str == 'FALSE' or str == 0 or str == false or str == 'n' then
			return false
		else
			return nil, '!!!ERROR: It is not boolean', 2
		end
	elseif tp == 't' then -- 时间
		if string.find(str, '^%s*%d%d:%d%d:%d%d%s*$') or
			string.find(str, '^%s*%d%d/%d%d/%d%d%s*$') or
			string.find(str, '^%s*%d%d/%d%d/%d%d d%d:%d%d:%d%d%s*$') then
			return str
		else
			return nil, '!!!ERROR: It is not boolean', 2
		end
	elseif tp == 'h' then -- Lua 哈希表结构
		print(str)
		return loadstring('return {'..str..'}')()
	elseif tp == 'm' then -- Lua 函数
		return loadstring('return '.. str)()
	else
		return nil, '!!!ERROR: Do no recognize this type ('..tp..')', 2
	end
end
--- 把str字符串，以sep为分隔符，以tp为类型，转化为数组；null指示是否可以为空，default指示是否为空填充默认值。如果失败，返回nil,'出错信息'；否则只返回一个值
local function str2array(str, sep, tp, null, default, allnull, fork, colname)
	local startIdx = 1
	local splitIdx = 1
	local array = {}
	local flag, m, l
	if str == nil then
		if null == true then
			if default == true then
				return array
			else
				return nil
			end
		else
			return nil, '!!!Error: Nil is not permitted', 2
		end
	end
	str=tostring(str)
	while true do
		local lastIdx = string.find(str, sep, startIdx)
	if not lastIdx then
		local ss = string.sub(str, startIdx, string.len(str))
		if allnull and ss=='' then ss = nil end
		local tmp, msg ,lv = str2type(ss, tp , allnull, default, fork, colname)
		if lv == 2 then return nil, msg, lv end
		if not flag and lv == 1 then
			flag = true
			m = msg;l=lv
		end
		array[splitIdx] = tmp
		break
	end
	local ss = string.sub(str, startIdx, lastIdx - 1)
		if allnull and ss=='' then ss = nil end
	local tmp, msg, lv = str2type(ss, tp, allnull, default, fork, colname)
	if lv == 2 then return nil, msg, lv end
	if not flag and lv == 1 then
		flag = true
		m = msg;l=lv
	end
	array[splitIdx] = tmp
	startIdx = lastIdx + string.len(sep)
	splitIdx = splitIdx + 1
	end
	return array, m, l
end

--- 读取Cells数据,如果有tp 则检查。如果失败，返回nil,'出错信息'；否则只返回一个值
local function ExcelReadCell(oExcel,row,column,tp,colname)
	if oExcel == nil then error("oExcel is not a object!") end
	check(row, column)
	coord = excelCoord(row,column)
	local value
	if oExcel[row] and oExcel[row][column] ~= nil then
		value=oExcel[row][column]
		if value=='' then oExcel[row][column], value = nil end
	end
	local flag, msg = emptyStr(value) -- 空串检查
	if msg and tp and not tp.allnull then
		print(msg..' ('..coord..') !!!')
		msg=nil
	end

	-- 如果tp（列属性）存在，则按属性处理数据
	if tp then
		if tp['array'] == true then
			local msg, lv
			value, msg, lv = str2array(value, tp['sep'], tp['type'], tp['null'], tp['default'],
				tp.allnull, tp.fork, colname)
			if lv == 1 then print(msg..' ('..coord..')!!!') end
			if lv == 2 then return nil, msg..' ('..coord..')!!!', lv end
		else
			local msg, lv
			value, msg, lv  = str2type(value, tp['type'], tp['null'], tp['default'], tp.fork, colname)
			if lv == 1 then print(msg..' ('..coord..')!!!') end
			if lv == 2  then return nil, msg..' ('..coord..')!!!', lv end
		end
	end
--[[	print('the value', row, column, oExcel.Activesheet.Cells(row, column).Value,
		oExcel.Activesheet.Cells(row, column).Value1,
		oExcel.Activesheet.Cells(row, column).Value2,
		oExcel.Activesheet.Cells(row, column).row,
		oExcel.Activesheet.Cells(row, column).column
	)]]--
	return value
end

--- 读取一行（简单单元格）如果count==nil，则读到第一个nil值为止。返回:总列数，所有列数据，服务器端列数，服务器数据，客户端列数，客户端所有数据
local function ExcelReadSimpleLine(oExcel, row, count, tp, colnames)
	local r,svr,client,c,y,z={},{},{},0,0,0
	if count == nil then
		while true do
			local tmp, msg, lv = ExcelReadCell(oExcel,row,c+1,
				tp and tp[c+1] or nil, colnames and colnames[c+1] or nil)
			if msg then error(msg,lv) end
			if tmp == nil then break end
			c = c +1
			if tp and tp[c+1]['server'] == true then
				y = y+1
				svr[y] = tmp
			end
			if tp and tp[c+1]['client'] == true then
				z = z+1
				client[z] = tmp
			end
			r[c] = tmp
		end
	else
		for i=1, count do
			local msg, lv
			c = c +1
			r[i], msg, lv=ExcelReadCell(oExcel,row,i,
				tp and tp[i] or nil, colnames and colnames[i] or nil)
			if msg then error(msg, lv) end
			if tp and tp[i]['server'] == true then
				y = y+1
				svr[y] = r[i]
			end
			if tp and tp[i]['client'] == true then
				z = z+1
				client[z] = r[i]
			end
		end
	end
	return c, r, y, y>0 and svr or nil, z, z>0 and client or nil
end

local commentMsg = ''
--- 判断一行是否是注释
local function isComment(line)
	if line[1] and string.find(line[1], "^%s*//.*") then
		local _,_,c = string.find(line[1], "^%s*//(%-%-.*)")
		if c then
			commentMsg = commentMsg..c..' '..table.concat(line, ' ', 2, #line)..'\n'
		end
		return true
	else
		return false
	end
end

--- 判断是否是结束行
local function isEnd(line)
	if line[1] and string.find(line[1], "^%s*///END%s*$") then
		return true
	else
		return false
	end
end

--- 找到Server文件路径指示符
local function svrFile(line)
	if line[1] then
		local _,_,tmp = string.find(line[1], "^%s*///SERVER FILE PATH:(.*)$")
		return tmp
	else
		return nil
	end
end
--- 找到Client文件路径指示符
local function clientFile(line)
	if line[1] then
		local _,_,tmp = string.find(line[1], "^%s*///CLIENT FILE PATH:(.*)$")
		return tmp
	else
		return nil
	end
end
--- 找到DB TABLE指示符
local function dbTable(line)
	if line[1] then
		local _, _, tmp = string.find(line[1], "^%s*///DB TABLE:(.*)$")
		return tmp
	else
		return nil
	end
end

--- 把一行转换为小写
local function lowcase(line)
	for i,v in pairs(line) do
		line[i]=string.lower(v)
	end
	return line
end

--- 分离出列名和列类型
---标志位
--[[
	p  ~ 主键
	i  ~ 整数
	f  ~ 浮点数
	s  ~ 字符串
	b  ~ 布尔值
	t  ~ 时间
	a  ~ 数组
	e  ~ 允许为空
	E  ~ 允许数组元素为空
	d ~ 用默认值代替空（数组：{}，数字：0，布尔：false，字符串：''）
	,  ~ 指定数组的分隔符为,
	|  ~ 指定数组的分隔符为|
	#  ~ 指定数组的分隔符为#
	\  ~ 指定数组的分隔符为回车
	-  ~ 需要翻译的字符串和数组
	<  ~ 只在客户端有用
	>  ~ 只在服务端有用
	1,2,3 ~ 若存在，指定客户端索引，替代主键，生成多级索引结构
	4,5,6 ~ 若存在，指定服务器索引，替代主键，生成多级索引结构
	h  ~ 哈希表（一个特殊结构，不检查正确与否，简单地将其内容作为一个表结构返回）
	m  ~ 函数（一个特殊结构，不检查正确与否，简单地将其内容作为一个函数返回）
--]]
local function apartNameType(line)
	local tp,l,name = {},{},{}
	local pFlag = false
	print("apartNameType info", line)
	for i,v in pairs(line) do
		print(i, v)
		tp[i] = {}
		_,_, l[i], tp[i]['attr']= string.find(v,"([%w_]+):([%w<>,|#\\%-123456]+)")
		if name[l[i]] then error('!!!Error: duplicate column name: '..l[i]..' !!!') end
		if not l[i] then error("!!! Error: not exist column：".. i) end
		name[l[i]] = true
		local coord = excelCoord(nil,i)
		if not l[i] or not tp[i]['attr'] then
			error('!!!Error: column partern has syntax error ('..coord..')!!!')
		end
		-- 分析类型
		for j=1, string.len(tp[i]['attr']) do
			str = string.sub(tp[i]['attr'], j, j)
			if str == 'p' then -- 主键
				if pFlag == true then
					error('!!!Error: Duplicate primary key find in column '..coord..' and '..excelCoord(nil,tp['pk']) )
				else
					pFlag = true
					tp['pk'], tp.pkname = i, l[i]
				end
			elseif str == 'i' or str == 'f' or str == 's' or str == 'b' or str == 't' or str == 'h' or str == 'm' then -- 类型
				if tp[i]['type'] == nil then
					tp[i]['type'] = str
				else
					error('!!!Error: Duplicate type in column '..coord..' ('..tp[i]['type']..' and '..str..')!!!')
				end
			elseif str == 'a' then-- 数组
				tp[i]['array'] = true
			elseif str == ',' or str == '|' or str == '#' or str == '\\' then -- 分隔符
				if tp[i]['sep'] == nil then
					if str == '\\' then
						tp[i]['sep'] = '\n'
					else
						tp[i]['sep'] = str
					end
				else
					error('!!!Error: Only 1 kind of separator required in column '..coord..'!!!')
				end
			elseif str == '-' then
				tp[i].fork = true
			elseif str == '>' then -- 服务器端
				tp[i]['server'] = true
			elseif str == '<' then -- 客户端
				tp[i]['client'] = true
			elseif str == 'e' then -- 允许为空
				tp[i]['null'] = true
			elseif str == 'E' then -- 允许为空
				tp[i]['null'] = true
				tp[i].allnull = true
			elseif str == 'd' then -- 允许空的情况下使用默认值
				tp[i]['default'] = true
			elseif str == '1' or str =='2' or str =='3' then -- 指定客户端索引
				tp['cindex'] = tp['cindex'] or {}
				tp['cindex'][tonumber(str)] = l[i]
			elseif str == '4' or str =='5' or str=='6' then -- 制定服务器端索引
				tp['sindex'] = tp['sindex'] or {}
				tp['sindex'][tonumber(str)-3] = l[i]
			else
				error('!!!ERROR: Do no support type: '..str..' in '..coord..'!!!')
			end
		end
		-- 没有类型
		if tp[i]['type'] == nil then error('!!!Error: Column '.. coord ..' has no type assigned!!!') end
		--数组没分隔符
		if tp[i]['array'] == true and tp[i]['sep'] == nil then error('!!!Error: Array has no separator assigned in column '.. coord .. ' !!!') end
		if tp[i].fork and tp[i].type ~= 's' then error('!!!Error: Fork type of column '..coord..' must be string') end
		-- 客户端或服务器端指定
		if tp[i]['server'] == nil and tp[i]['client'] == nil then error('!!!Error: Column '..coord..' has neither server or client side assigned!!!') end
		-- 客户端索引在服务器列
		if tp['cindex'] and tp[i]['server'] and tp[i]['client'] == nil and (l[i] == tp['cindex'][1] or l[i] == tp['cindex'][2] or l[i] == tp['cindex'][3]) then
			error('!!!Error: Column '..coord..' has client index on only server column!!!')
		end
		-- 服务端索引在客户端列
		if tp['sindex'] and tp[i]['client'] and tp[i]['server'] == nil and (l[i] == tp['sindex'][1] or l[i] == tp['sindex'][2] or l[i] == tp['sindex'][3]) then
			error('!!!Error: Column '..coord..' has server index on only client column!!!')
		end
	end
	-- 没有主键
	if pFlag == false then error('!!!Error: No column is primary key!!!') end

	return l,tp
end

--- 读取列数,列名,列标识开始的行，列属性，服务器端属性，客户端属性（列名行为第一行非注释的行）
local function ExcelReadCol(oExcel)
	local rcount = 1
	local sf,cf, db
	while true do
		local count, line = ExcelReadSimpleLine(oExcel, rcount)
		if isComment(line) then
			if isEnd(line) then return nil end
			rcount = rcount +1
			sf = sf or svrFile(line)
			cf = cf or clientFile(line)
			db = db or dbTable(line)
		else
			local tp
			local svr,client,y,z={},{},1,1
			line, tp = apartNameType(line)
			for i,v in next, line do
				if tp[i]['server'] == true then
					svr[y]=line[i]
					y=y+1
				end
				if tp[i]['client'] == true then
					client[z]=line[i]
					z=z+1
				end
			end
			if sf == nil or cf == nil then
				print("Server file: ",sf,"client file: ",cf,"db table: ",db)
				error('!!!ERROR: Server file path, client file path do not assigned!!!')
			end
			return count, line, rcount, tp, svr, client, sf, cf, db
		end
	end
	return nil
end

--- 将第一行为Column名字的表转换为每行带哈希键值的表
local function IndexData(tb)
	if tb == nil then return nil end
	local t={}
	for i=2, #tb do
		t[i-1]=tb[i]
		for j=1, #tb[1] do
			t[i-1][tb[1][j]]=t[i-1][j]
			t[i-1][j] = nil
		end
	end
	tb = nil
	return t
end

--- 读取一个Sheet，返回总行数，服务器端数据表，客户端数据表 （简单单元格）
local function ExcelReadSheet(excel, sheet, name, clipath, svrpath)
	commentMsg = ''
	local svr, client, colcount, i, rowcount, tp, colname = {},{}
	sheetcache = sheet
	local sf,cf,db
	-- 先找到列名行
	colcount, colname, i, tp, svr[1], client[1],sf,cf,db = ExcelReadCol(sheet)
	if colcount == nil then return 0, nil end
	forkFile(excel..' : '..svrpath..'/'..sf)
	forkFile(excel..' : '..clipath..'/'..cf)
	local pk = colname[tp['pk']]
	rowcount = 0
	while true do
		local tmp = { ExcelReadCell(sheet, i+1, 1) }
		if isComment(tmp) then
			if isEnd(tmp) then break end
		else
			rowcount = rowcount + 1
			_, _, _, svr[rowcount+1], _, client[rowcount+1] =
				ExcelReadSimpleLine(sheet, i+1, colcount, tp, colname)
		end
			i = i+1
	end
	if rowcount < 1 then
		return nil
	end
	if #svr > 1 then
		svr = indexPK(IndexData(svr),pk)
	else
		svr = nil
	end
	if #client > 1 then
		client = indexPK(IndexData(client),pk)
	else
		client = nil
	end
	return rowcount, svr, client,sf,cf,db,tp, colname, pk
end

local forks0 = { [0]=0 }
--- 序列化
local function serialize(o, file, deep, forks, fork)
	if type(o) == "number" then
		file:write(('%.12g'):format(o))
	elseif type(o) == "string" then
		-- file:write((('%s%q'):format(fork and o:find'[\128-\255]' and '__' or '', o):gsub('\n', 'n')))
		if opent and fork and o:find'[\128-\255]' then
			file:write((('%s%q%s'):format('('.._tkey..'', o, ')'):gsub('\n', 'n')))
		else
			file:write((('%q'):format(o):gsub('\n', 'n')))
		end
	elseif type(o) == "boolean" then
		file:write(o and 'true' or 'false')
	elseif type(o) == "table" then
		local isIntTable = table.isIntTable(o)
		if isIntTable then
			file:write("[\n")
		else
			file:write("{\n")
		end
		-- file:write("{")
		local tn,i={},1
		for k,v in pairs(o) do
			tn[i] = k
			i=i+1
		end
		table.sort(tn, function(a,b)
			if type(a) ~= type(b) then
				return type(a) < type(b)
			else
				return a < b
			end
		end)
		for j=1,#tn do
			local v,k = o[tn[j]],tn[j]
			if type(k) == 'number' then -- 数字
				-- file:write(" [",k,"] : ")
			elseif type(k) == 'string' and string.find(k,'^[a-zA-Z_][a-zA-Z0-9_]*$') then -- 正常变量
				file:write( ('%q'):format(k), " : ")
			else -- 其他字符
				if opent and forks[deep] then
					file:write(((' [%s%q%s] : '):format('('.._tkey..'', k, ')'):gsub('\n', 'n')))
				else
					file:write(((' [%q] : '):format(k):gsub('\n', 'n')))
				end
				-- file:write(((' [%s%q] = '):format(forks[deep] and '__' or '', k):gsub('\n', 'n')))
			end
			serialize(v, file, deep+1, deep <= forks[0] and forks or forks0,
				fork or deep==forks[0]+1 and forks[k])
			if j==#tn then
				file:write("\n")
			else
				file:write(",\n")
				-- file:write(",")
			end
		end
--		for k,v in pairs(o) do
--			file:write(" [",k,"] =")
--			serialize(v, file, deep+1)
--			file:write(",")
--		end
		-- if deep < 3 then
		-- 	file:write("}\n")
		-- else
		-- 	file:write("}\n")
		-- end
		if isIntTable then
			file:write("]")
		else
			file:write("}")
		end
	else
		error("cannot serialize a " .. type(o))
	end
end

--- 获取文件路径和文件名
-- @param filePath 文件路径
-- @param pattern 路径分隔符
local function getPathAndName( filePath,pattern )
    local b, last = 0, 0
	local path, fileName  = '', ''
    while true do
        local s,e = string.find( filePath, pattern, b)
        if s == nil then break
        else last = s end
         b = s + string.len(pattern)
    end
    path = string.sub( filePath, 1, last )
	fileName = string.sub(filePath, last+1, -1)
	local _,dotend = string.find(fileName, '%.')
	return path, string.sub(fileName, 1, dotend-1)
end

--- 对比两个table的数据,如果tb1属于tb2则返回true
function containTable(tb1, tb2, lv)
	local msg=''
	if type(tb1) ~= 'table' and type(tb2) ~= 'table' then
		if tb1 ~= tb2 then
			return false, msg
		else
			return true
		end
	elseif type(tb1) == 'table' and type(tb2) == 'table' then
		local flag=true
		for k,v in pairs(tb1) do
			local ret = containTable(tb1[k], tb2[k], lv+1)
			if ret == false and lv <2 then
				msg = msg..'Table not equal, key = '..k..'\n'
			end
			if ret == false then flag=false end
		end
		return flag, msg
	else
		return false, msg
	end
end

--- 对比Excel文件和原来的生成文件，看原来的数据是否有更新
function compareUpdate(file, tb)
	local old = loadfile(file)
	if old then
		print(file,' exists.')
		local tbold = old()
		local ret1, msg1 = containTable(tbold, tb, 1)
		local ret2, msg2 = containTable(tb, tbold, 1)
		if ret1==true and ret2==true then
			print(file, ' has no update.')
			return true
		else
			print(msg1)
			print(msg2)
			return false
		end
	end
	return nil
end

--- 表合并,如果两表有重叠，第二个参数返回tb2的重叠部分
function mergeTable(tb1, tb2)
	local newtb = {}
	tb1 = tb1 or {}
	tb2 = tb2 or {}
	local dup, count = {} , 0
	for i, v in pairs(tb1) do
		newtb[i] = tb1 [i]
		tb1[i] = nil
	end
	for i, v in pairs(tb2) do
		if newtb [i] then -- 两个表有重复部分
			dup[i] = tb2[i]
			count = count +1
			tb2[i] = nil
		else
			newtb[i] = tb2[i]
			tb2[i] = nil
		end
	end
	return newtb, count > 0 and dup or nil
end

local function insertMerge(flist, fname, dlist, data, dblist, dbname, tplist, tp, pks, pk)
	local num = 0
	local dup
	for i, v in pairs(flist) do
		num = num+1
		if flist[i] == fname then
--			print("Starting merge same name table ",fname)
			dlist[i], dup = mergeTable(dlist[i],data)
			if dup then -- 有重复行
				for k, _ in pairs(dup) do
					error("Please check duplicate key "..tostring(k))
				end
			end
--			print("Complete merge same name table ",fname)
			return flist, dlist, dblist, tplist, pks
		end
	end
	flist[num+1] = fname
	dlist[num+1] = data
	if dblist then dblist[num+1] = dbname end
	tplist[num+1] = tp
	if pks then pks[num+1]=pk end
	return flist, dlist, dblist, tplist
end

function multiIndexLine(linekey, line, tb, ...)
	local keys={...}
	if #keys == 1 then
		local key = keys[1]
		local keyv = line[key]
		if keyv == nil then
			error(key..' does not exists in '..linekey)
		end
		if type(keyv) == 'table' then
			for i,v in pairs(keyv) do
				if tb[v] then
					error('duplicate '..key..'='..v..' in '..linekey)
				end
				tb[v] = line
			end
		else
			if tb[keyv] then
				error('duplicate '..key..'='..keyv..' in '..linekey)
			end
			tb[keyv] = line
		end
		return
	end
	local key = keys[1]
	local keyv = line[key]
	if keyv == nil then
		error(key..' does not exists in '..linekey)
	end
	table.remove(keys,1)
	if type(keyv) == 'table' then
		local c=0
		for i,v in pairs(keyv) do
			tb[v] = tb[v] or {}
			multiIndexLine(linekey, line, tb[v], unpack(keys))
			c=c+1
		end
			if c == 0 then
				error(key..' does not exists in '..linekey)
			end
	else
		tb[keyv] = tb[keyv] or {}
		multiIndexLine(linekey, line, tb[keyv], unpack(keys))
	end
end

function multiindex(tb,...)
	local newtb = {}
	for k,v in pairs(tb) do
		multiIndexLine(k,v,newtb,...)
	end
	return newtb
end

local function multiIndex(tb, tp, side, forks)
	local newtb={}
	local sideidx
	if side == 'server' then
		sideidx = tp['sindex']
	elseif side == 'client' then
		sideidx = tp['cindex']
	else
		error('wrong paramater')
	end

	for i = 1, #forks do forks[i] = nil end
	if sideidx then
		forks[0] = #sideidx
		for i = 1, #sideidx do
			if forks[sideidx[i]] then forks[i] = true end
		end
		return multiindex(tb,unpack(sideidx))
	else
		forks[0] = 1
		if forks[tp.pkname] then forks[1] = true end
		return tb
	end
end

local function copyFile(from, to)
	local fromf = assert(io.open( from, 'r'))
	local tof = assert(io.open(to,'w'))
	local buff = fromf:read('*all')
	tof:write(buff)
	fromf:close()
	tof:close()
end
--- 转换一个Excel文件为对应的Server、Client端Lua和Server端SQL文件

local d = {}
local function convert(excel, sheets, cliPath, svrPath, name )
	print("\n===================== Convert starting:"..excel)
	local svrdata,clidata,svrfile,clifile,dbfile,svrtp,clitp, pks = {},{},{},{},{},{},{},{}
	local forks, svrsheetnames, clisheetnames = {}, {}, {}

	for n, s in pairs(sheets) do
		if string.find(n,'^%#') then -- sheet名称必须以#开头才转
			print('Start convert sheet '..n)
			local count,sd,cd,sf,cf,db,tp,colname, pk = ExcelReadSheet(excel, s, n, cliPath, svrPath)
			if count>0 then
				svrfile, svrdata, dbfile, svrtp = insertMerge(svrfile, sf, svrdata, sd, dbfile, db, svrtp, tp)
				clifile, clidata, _, clitp = insertMerge(clifile, cf, clidata, cd, nil, nil, clitp, tp, pks, pk)

				svrsheetnames[#svrsheetnames + 1] = n
				clisheetnames[#clisheetnames + 1] = n
			end
			local fs = forks[sf] or {}
			forks[sf], forks[cf] = fs, fs
			for i = 1, #tp do
				if tp[i].fork then fs[colname[i]] = true end
			end
		else
			print('Neglect sheet '..n)
		end
	end
	--[==[ -- 暂时屏蔽server的生成
	print("\n--------------------------------- server side and database")
	for i in pairs(svrdata) do
		svrdata[i]=multiIndex(svrdata[i],svrtp[i],'server', forks[svrfile[i]])
		local file = (os.info.sconf or svrPath)..'/'..svrfile[i], out
		if loadfeast then
			loadfeast = file:sub(4) --客户端配置文件
			out = { write=table.push, close=table.concat }
		else
			out = os.info.cconf and { write=assert, close=assert } or assert(io.open(file, 'wb'))
		end
		out:write(commentMsg)
		local svername = string.sub(svrfile[i], 1, -5)
		out:write('_G.' ..svername..' = \n')
		serialize(svrdata[i], out, 1, forks[svrfile[i]])
		out:write("return ".. svername .. ", '"..pks[i].."'")
		if loadfeast then for i = 1, 100 do print(out[i]) end end
		out = out:close()
		if loadfeast then zipdatas[loadfeast] = out end
		print(file, ' server side configure file created')
	end
	--]==]
	print("\n--------------------------------- client side ")
	for i in pairs(clidata) do
		clidata[i]=multiIndex(clidata[i],clitp[i],'client', forks[clifile[i]])
		local file = (os.info.cconf or cliPath)..'/'..clifile[i], out
		print('------------file', file, '~')
		dump(clifile)
		if not loadfeast then --or ret1 == nil or ret1 == false then
			out = os.info.sconf and { write=assert, close=assert } or assert(io.open(file, 'wb'))
			out:write(commentMsg)
			local cliname = string.sub(clifile[i], 1, -5)
			-- out:write("--excelname="..excel..":")
			-- out:write("sheetname="..(clisheetnames[i] or '').."\n")
			-- out:write('_G.'..cliname..' = { \n')
			-- out:write('{\n')
			dump(clidata[i])
			serialize(clidata[i], out, 1, forks[clifile[i]])
			-- out:write(", '"..pks[i].."' }")
			out:close()
			print(file, ' client side configure file created ')
		end
	end
	print("\n===================== Convert completed ")
end
--- 读取配置文件
local function readLuaConf()
	local confpath = CONFPATH or 'conf.lua'
	print("---------------------read configure.",confpath)
	local pCfg = assert(loadfile(confpath))
	print("---------------------read complete.",confpath)
	return pCfg()
end

function _G.convertAll()
	print('\n+++++++++++++++++++++++++++++++++++++++++ ALL START+++++++++++++++++++++++++++\n')
	local conf, all = readLuaConf(), {}
	local xlss = loadfeast and conf.feasts or conf.xlsFileList
	local curPath = getPathAndName(os.info[0], "\\")
	if string.find(conf.xlsFilePath,'^%.') then -- 相对路径
		conf.xlsFilePath = curPath..'\\'..conf.xlsFilePath
	end
	for i, xls in ipairs(xlss) do
		local t = _excel(conf.xlsFilePath..'\\'..xls)
		for s, ds in pairs(t) do
			for j, d in ipairs(ds) do
				for k, s in ipairs(d) do
					d[k] = tostring(s)
				end
			end
		end
		all[i] = t
	end
	for i, xls in ipairs(xlss) do
		convert(xls, all[i], conf.clientConfigPath, conf.serverConfigPath, n)
	end
end

----------------- fork -----------------

local path = {
	-- '../server', '../client', -- '../data',
}
local sconf = {
}
local cconf = {
}
local fork = {
	-- 'sg.nyugame.com.lua',
}
local exclude = {

}

local texts, textis, textqs, textks = {}, {}, {}, {}

function forkFile(file)
	table.push(texts, false, false, false)
	table.push(textqs, '', '--------------- '..file..' ---------------', '')
	table.push(textks, false, false, false)
end

function forkText(text, key)
	local x = #texts+1
	texts[x] = text
	local i = textis[text]
	if i then
		textqs[x], textks[x] = textqs[i], textks[i]
	else
		textis[text] = x
		textqs[x] = ('%q'):format(text):gsub('\n', 'n')
		textks[x] = key or false
	end
end

local function str(ls, lua, h, j, k)
	local l, s = lua:sub(h, j-1), lua:sub(j, k)
	local _ = j < k and (l:find(_tkey..'%s*$') or l:find'_TTT%s*$')

	if opent and ( _ or s:find'[\128-\255]' ) then
		forkText(loadstring('return '..s)())
		if not _ then
			s = '('.._tkey..''..s..')'
		elseif _ and l:find'_TTT%s*$' then
			l = l:gsub('_TTT', '')
			s = '('.._tkey..''..s..')'
		elseif _ and l:find(_tkey..'%s*$') and not l:find('%('.._tkey..'%s*$') then
			if lua:sub(j-2, j-1)==_tkey then
				l = lua:sub(h, j-3)
			end
			s = '('.._tkey..''..s..')'
		end
	end
	table.push(ls, l, s)
end

local function parse(file)
	local lua, err = io.readall(file)
	if not lua then print(err) return end
	print(file)
	forkFile(file)
	local ls, h, i, j, k = {}, 1, 1
	while true do
		j = lua:find('[%-%[\'"]', i)
		if not j then break end
		if lua:find('^%-%-', j) then
			local a = lua:match('^%[(=*)%[', j+2)
			if a then
				k = select(2, lua:find(']'..a..']', j+2)) or #lua
			else
				k = lua:find('\n', j+2) or #lua
			end
			ls[#ls+1] = lua:sub(h, k) h = k+1
		elseif lua:find('^[\'"]', j) then
			local a = '['..lua:sub(j, j)..'\n\\]'
			k = j-1
			repeat
				k = lua:find(a, k+2)
			until not (k and lua:find('^\\', k))
			if not k or lua:find('^\n', k) then
				ls[#ls+1] = lua:sub(h, k or #lua) h = (k or #lua)+1
			else
				str(ls, lua, h, j, k) h = k+1
			end
		else
			local a = lua:match('^%[(=*)%[', j)
			if a then
				k = select(2, lua:find(']'..a..']', j+2))
				if not k then break end
				str(ls, lua, h, j, k) h = k+1
			else
				k = j
			end
		end
		i = k+1
	end
	if h <= #lua then ls[#ls+1] = lua:sub(h, #lua) end
	local t = table.concat(ls)
	if t == lua then return end
	print'\t\t\tchange'
	local f, err = io.open(file, 'wb')
	if f then
		f:write(t) f:close()
	else
		print(err)
	end
end
local function isDir(path)
  local file = io.open(path, "rb")
  if file then file:close() end
  return file==nil
end


texts[1], textqs[1] = false, 'return {'
local function isConfFile( path )
	local f=io.open (path, "r") --打开你的输入文件 名称为youfile.txt
	local line = f:read("*l") --读取一行，但是不保存
	-- print(path, line)
	if path:tail'.lua' and line and line:find('%-%-') and line:find('excelname') then
		f:close() --关闭
		return true
	end
	f:close()
	return false
end
local function enumdir(p, s)
	for file in next, io.dir(p) do
		file = p..'/'..file
		if isDir(file) then
			enumdir(file, s)
		else
			if file:tail'.lua' and not isConfFile(file) then
				if not exclude[file] then s[#s+1] = file end
			end
		end
	end
end

for _, p in ipairs(path) do
	local s = {}
	enumdir(p, s)
	table.sort(s)
	for i, file in ipairs(s) do
		if file:tail'.lua' then parse(file) end
	end
end
for _, file in ipairs(sconf) do
	parse('../server/config/'..file)
end
for _, file in ipairs(cconf) do
	parse('../client/config/'..file)
end
---------------- excel ----------------
convertAll()


table.push(texts, false, false)
table.push(textqs, '', '}')
table.push(textks, false, false)


print('\n======= END ========')
-- os.exit()
