--table扩展
local type = type
local next = next
--talbe.sort(t,table.ASC) talbe.sort(t,table.asc('k'))
function table.ASC(a,b) return a<b end
function table.asc(k) return function(a,b) return a[k]<b[k] end end
function table.DESC(a,b) return a>b end
function table.desc(k) return function(a,b) return a[k]>b[k] end end

function _G.inext(t, i)
	i = i==nil and 1 or int(i)+1
	t = rawget(t, i)
	if t==nil then return end
	return i, t
end

local visited;
function table.singlefind(tb, func, path)
	for k, v in next, tb do
		if type(v)=='table' then
			if not  visited[v] then
				visited[v]=true;
				if func(v) then
					table.insert(path, k);
					return true;
				else
					table.insert(path, k);
					if table.singlefind(v, func, path) then
						return path;
					end
					path[#path]=nil;
				end
			end
		end
	end
	return false;
end

function table.template( tb )
	local s, loop = table.tostring2( tb )
	if loop then
		if not PUBLIC then
			assert( "template loop table" )
		end
		return loadstring( 'return string.totable[===[' .. s ..']===]' )
	else
		return loadstring( 'return ' .. s )
	end
end

local tpns = { }
function table.templaten( n )
	if n <= 0 then return { } end
	if tpns[n] then return tpns[n]( ) end
	local tb = { }
	for ii = 1, n do
		tb[ii] = 0
	end
	local tpn = table.template( tb )
	tpns[n] = tpn
	return tpn( )
end

function table.tocsv(tb)
	local t={};
	for ii=1, #tb do
		local line=tb[ii];
		if type(line)=='table' then
			local newline=table.copy({}, line);
			for k, v in next, newline do
				if type(v)=="string" and string.find(v, ",") then
					newline[k]='"'..table.concat(string.split(v, '"'), '""')..'"';
				end
			end
			table.insert(t, table.concat(newline, ','));
		else
			error("error format for csv");
		end
	end
	return table.concat(t, "\r\n");
end

function table.filter(tb, func)
	local newtb={};
	for k, v in next, tb do
		if func(v) then
			newtb[k]=v;
		end
	end
	return newtb;
end
function table.each(tb, func)
	for k, v in next, tb do
		func(v, k)
	end
end

function table.map(tb, func)
	local t={}
	for k, v in next, tb do
		t[k] = func(v, k)
	end
	return t
end

function table.cover( dest, src )
	local samek -- true要剪纸
	for k, v in next, src do
		if dest[k] ~= v then
			if dest[k] == nil then
				samek = false
			elseif samek == nil then
				samek = true
			end
			dest[k] = v
		end
	end
	return dest, samek
end

function table.find(tb, func)
	visited={};
	return table.singlefind(tb, func, {});
end

function table.add( tb, col, newcol )
	local v = 0
	for ii = 1,#tb do
		v = v + tb[ii][col]
		tb[ii][newcol] = v
	end
	return tb
end

function table.ikey(tb, v)
	for ii=1, #tb do
		if tb[ii] == v then return ii; end
	end
end

if not table.clear then
function table.clear( t )
	if not t then return t end
	for k, v in next, t do
		t[k]=nil;
	end
	return t
end
end

function table.pushv( dest, src )
	for k, v in next, src do
		table.insert( dest, v )
	end
	return dest
end

function table.removev(tb, value)
	for k, v in next, tb do
		if v==value then table.remove(tb, k) return end
	end
end

function table.recursive(dest, src)
	for k, v in next, src do
		if type(v) == "table" and type(dest[k]) == "table" then
			table.recursive( dest[k], v )
		else
			dest[k] = v
		end
	end
	return dest
end
function table.shuffle(s, n)
	for i = 1, math.min(n, #s) do
		local j = math.random(i, #s)
		s[i], s[j] = s[j], s[i]
	end
	return s
end

function table.del(tb, dels)
	for k, v in next, dels do
		if type(v)=='table' then
			table.del(tb[k], v);
		else
			tb[k]=nil;
		end
	end
end

function table.readonly(x, name, deep)
	if deep then
		for k, v in next, x do
			if type(v)=='table' then
				x[k] = table.readonly(v, name..'.'..k, true)
			end
		end
	end
	local m = {}
	m.__newindex = function () error(name and 'readonly '..name or 'readonly') end
	return setmetatable(x, m), m
end

--[[function table.copy ( t1, t2 )
	if t2 == nil then return t1; end;
	for  k, v in next, t2 do
		t1[k] = v;
	end
	return t1;
end]]

function table.copyAll ( t1, t2 )
	local t = t2;
	while t do
		table.copy( t1, t );
		t = getmetatable(t2);
	end
	return t1;
end

function table.keys( t, keys )
	local keys = keys or { }
	for k, _ in next, t do
		keys[#keys+1] = k
	end
	return keys
end

function table.ikeys(t)
	local s = {}
	for i, v in ipairs(t) do
		s[i] = i
	end
	return s
end

function table.values(t)
	local values = {};
	for _, v in next, t do
		table.insert( values, v );
	end
	return values;
end

function table.compare( t1, t2 )
	t1 = t1 or EMPTY
	t2 = t2 or EMPTY
	local com, r1, r2 = {}, table.append({}, t1), table.append({}, t2);
	for k, v in next, t1 do
		if t2[k] == v then
			com[k] = v;
			r1[k] = nil;
			r2[k] = nil;
		end
	end
	return com, r1, r2;
end

function table.reverse( t, func )
	local r = {};
	for k, v in next, t do
		r[v] = func and func(k) or k;
	end
	return r;
end

function table.collect(t)
	local arr = {}
	for ii = 1, table.maxn(t) do
		if t[ii] then
			table.insert(arr, t[ii]);
		end
	end
	return arr;
end

function table.minn(t)
	for ii = 1, #t do
		if t[ii] then
			return ii;
		end
	end
end

function table.minv( tb, key )
	local minv
	for _, v in next, tb do
		local rv = key and v[key] or v
		minv = minv and math.min( minv, v ) or rv
	end
	return minv
end

function table.min( t )
	local firstk
	for k, _ in ipairs(t) do
		if t[k] then
			firstk = k
		break end
	end
	local minnum = t[firstk]
	local pos = firstk
	if not minnum then return end
	for k, v in ipairs(t) do
		if v < minnum then
			minnum = v
			pos = k
		end
	end
	return minnum, pos
end
function table.max( t )
	local firstk
	for k, _ in next, t do
		if t[k] then
			firstk = k
			break
		end
	end
	local maxnum = t[firstk]
	local pos = firstk
	assert( maxnum, 'blank table' )
	for k, v in next, t do
		if v > maxnum then
			maxnum = v
			pos = k
		end
	end
	return maxnum, pos
end

function table.count(t)
	local sum = 0
	for k,v in next,t do
		if v~=nil then
			sum = sum + 1
		end
	end
	return sum
end
function table.addValue(base,app)
	for k,v in pairs(app) do
		if type( v ) == 'number' then
			base[k] = (base[k] or 0) + v
		else
			base[k] = table.addValue( base[k] or { }, v )
		end
	end
	return base
end

--- 将平面表恢复为原有的表结构
local function unserialize(flattb, key)
	local subtb = flattb[key]
	for k, v in next, subtb do
		if type(v) == 'string' and v:find('__tb__') then
			subtb[k] = unserialize(flattb, v)
		else
			subtb[k] = v
		end
	end
	return subtb
end

--- 将平面结构的表是字符串转化为表
function string.totable( s )
	local flattb = loadstring( 'return ' .. s )()
	if flattb.__loop then --多个嵌套表组成的字符串
		local tb = {}
		for k, v in next, flattb['__tb__0'] do
			if type(v) == 'string' and v:find('__tb__') then
				tb[k] = unserialize(flattb, v)
			else
				tb[k] = v
			end
		end
		return tb
	else
		return flattb
	end
end

---平面化table
function table.flat( t, flattb, visited )
	local tbname = '__tb__' .. flattb.lv
	if not visited[t] then --没有遍历过
		-- flattb[tbname] = { __ref__ = {}}
		flattb[tbname] = {}
		visited[t] = tbname
		for k, v in next, t do
			if type(v) ~= 'table' then
				flattb[tbname][k] = v
			else
				if not visited[v] then
					flattb.lv = flattb.lv+1
					local newtbname = '__tb__' .. flattb.lv
					flattb[tbname][k] = newtbname
					-- flattb[tbname].__ref__[k] = newtbname
					table.flat( v, flattb, visited )
				else
					flattb.__loop = true
					flattb[tbname][k] = visited[v]
					-- flattb[tbname].__ref__[k] = visited[v]
				end
			end
		end
	end
end

function table.serialize ( o, s, nonumkey )
	if type(o) == "number" then
		s = table.concat({ s, o} )
	elseif type(o) == "string" then
		s = table.concat( { s, "'", o, "'" } )
	elseif type(o) == "boolean" then
		s = table.concat( { s,( o and "true" or "false" ) } )
	elseif type(o) == "table" then
		s = table.concat( { s, "{" } )
		for k, v in next, o do
			if type(k) == 'number' then
				if not nonumkey then s = table.concat( { s, "[", k, "]=" } ) end
				s = table.serialize( v, s, nonumkey );
				s = table.concat( { s, "," } )
			elseif type(k) == 'string' then
				s = table.concat( { s, "['", k, "']=" } )
				s = table.serialize( v, s, nonumkey );
				s = table.concat( { s, "," } )
			elseif type(k) == "boolean" then
				s = table.concat( { s, "[", ( k and "true" or "false" ), "]=" } )
				s = table.serialize( v, s, nonumkey );
				s = table.concat( { s, "," } )
			end
		end
		if s:find(',$') then
			s = string.sub(s, 1, -2)
		end
		s = table.concat( { s, "}" } )
	elseif type(o) == "function" then
	elseif type(o) == "userdata" then
	else
		error("cannot serialize a " .. type(o))
	end
	return s
end

local serializetable = {}
function table.serialize2( o, tb )
	if not tb then table.clear(serializetable) end
	tb = tb or serializetable
	if type(o) == "number" then
		table.push(tb, o)
	elseif type(o) == "string" then
		table.push(tb, "'", o, "'")
	elseif type(o) == "boolean" then
		table.push(tb, ( o and "true" or "false" ))
	elseif type(o) == "table" then
		table.push(tb, "{")
		for k, v in next, o do
			if type(k) == 'number' then
				table.push(tb, "[", k, "]=")
				tb = table.serialize2( v, tb );
				table.push(tb, ",")
			elseif type(k) == 'string' then
				table.push(tb, "['", k, "']=")
				tb = table.serialize2( v, tb );
				table.push(tb, ",")
			elseif type(k) == "boolean" then
				table.push(tb, "[", ( k and "true" or "false" ), "]=")
				tb = table.serialize2( v, tb );
				table.push(tb, ",")
			end
		end
		table.push(tb, "}" )
	elseif type(o) == "function" then
	elseif type(o) == "userdata" then
	else
		error("cannot serialize a " .. type(o))
	end
	return tb
end

function table.tonumber( t )
	local t2 = {}
	for k, v in next, t do
		t2[k]=tonumber(v);
	end
	return t2;
end

local flattb, visiteds = {}, {}
function table.tostring( t, nonumkey ) --紧凑字串
	table.clear(flattb)
	table.clear(visiteds)
	flattb.lv = 0
	table.flat( t, flattb, visiteds )
	flattb.lv = nil
	if not flattb.__loop then --没有循环引用的表,直接序列化
		return table.serialize( t, "", nonumkey ), false;
	else
		-- flattb.loop = nil
		local newstr = table.serialize( flattb, "", nonumkey );
		table.clear(flattb)
		table.clear(visiteds)
		return newstr, true;
		-- return serialize( flattb, "" ), true;
	end
end

local flattb2, visited2 = {}, {}
function table.tostring2( t ) --紧凑字串
	table.clear(flattb2)
	table.clear(visited2)
	flattb2.lv = 0
	table.flat( t, flattb2, visited2 )
	flattb2.lv = nil
	if not flattb2.__loop then --没有循环引用的表,直接序列化
		local tb = table.serialize2( t )
		return table.concat(tb), false;
	else
		local tb = table.serialize( flattb2 );
		local newstr = table.concat(tb)
		table.clear(flattb2)
		table.clear(visited2)
		return newstr, true;
	end
end


function table.minn(t)
	for ii = 1,#t do
		if not t[ii] then
			return ii
		end
	end
	return #t+1
end

function table.tostr(t, tabnum, float) --树形字串
	local tabnum = tabnum or 1
	local tabs = ''
	for i = 1, tabnum do
		tabs = tabs .. '\t' end
	local tt = type(t)
	local tostrt0 = {}
	local tostrts = {}
	assert(tt=='table','bad argument #1(table expected, got '..tt..')')
	local t0 = table.keys( t, tostrt0 )
	table.sort( t0, function( a, b )
		if type(a) == 'number' and type(b) == 'number' then
			return a<b
		elseif type(a) == 'number' and type(b) ~= 'number' then
			return true
		elseif type(b) == 'number' and type(a) ~= 'number' then
			return false
		else
			return tostring(a) < tostring(b)
		end
	end )

	for i, k in ipairs( t0 ) do
		local v = t[k]
		local tv = type(v)
		local tk = type(k)
		if tk=='number' then k = '['..k..']' end
		if tk=='string' then k = '["'..k..'"]' end--k:"123" ->> ["123"] =
		k = tabs .. k
		if tv=='table' then tostrts[#tostrts+1] = k..'='..table.tostr(v, tabnum + 1,float)
		elseif tv=='string' then
			if not _G.stringflag then
				tostrts[#tostrts+1] = k.."='"..v.."'"
			else
				tostrts[#tostrts+1] = k..'=[['..v..']]'
			end

		elseif tv=='number' then
			if v~=toint(v) and float then
				assert(float>=0 and toint(float)==float,'')
				tostrts[#tostrts+1] = k..string.format("=%."..float.."f",v)
			else
				tostrts[#tostrts+1] = k..'='..tostring(v)
			end
		else tostrts[#tostrts+1] = k..'='..tostring(v) end
	end
	local retstr = '{\n' .. table.concat( tostrts, ',\n' ) .. '\n' .. tabs .. '}'
	table.clear(tostrt0)
	table.clear(tostrts)
	return retstr
end

if not table.duplicate then
	Log.sys("This is the old fancy-server")

	--table.copy(dt, st)浅copy
	function table.clone(dt, st)	--深copy
		if type(st) ~= 'table' then
			error('source is not table in table.clone')
		else
			for k, v in next, st do
				if type(v) ~= 'table' then
					dt[k] = v
				else
					local temp = {}
					dt[k] = temp
					table.clone(temp, v)
				end
			end
		end
		return dt
	end

	function table.duplicate( tb )
		return table.copy( {}, tb )
	end
	function table.newclone( tb )
		return table.clone( {}, tb )
	end
else
	Log.sys("This is the new fancy-server")
	function table.newclone( st ) --深copy
		local dt = table.duplicate( st )
		if type(st) ~= 'table' then
			error('source is not table in table.clone')
		else
			for k, v in next, st do
				if type(v) == 'table' then
					dt[k] = table.newclone( v )
				end
			end
		end
		return dt
	end

	--table.copy(dt, st)浅copy
	function table.clone(dt, st)
		dt = table.newclone(st)
		return dt
	end
end

function table.replace( t, r ) --  处理buff
	for k, v in next, t do
		if type( v ) == "string" then
			local a ={};
			for s in string.gmatch( v, "var%d+" ) do
				table.insert( a, s );
			end
			if #a == 1 and a[1] == v then
				local index = string.find( v, "%d" );
				local value = r[ "var"..string.sub( v, index ) ];
				if not value then
					Log.fatal( r.id, "没有定义", "var"..string.sub( v, index ) );
				end
				t[k] = value;
			end
		elseif type( v ) == "table" then
			t[k] = table.replace( v, r );
		end
	end
	return t;
end

function table.delEmpty(tbl)
	if not next(tbl) then
		return nil
	end
	for k, v in next, tbl do
		if type(v) == 'table' then
			tbl[k] = table.delEmpty(v)
		end
	end
	return tbl
end

local function hequal(tb1 ,tb2)
	if table.count(tb1)~=table.count(tb2) then
		local diffkey
		for k, v in next, tb1 do
			if tb2[k] == nil then diffkey = k break end
		end
		if not diffkey then
			for k, v in next, tb2 do
				if tb1[k] == nil then diffkey = k break end
			end
		end
		return false, 'key num not same:'.. diffkey
	end
	for k1, v1 in next, tb1 do
		local isequal, info = table.equal(v1, tb2[k1])
		if not isequal then
			return false, info
		end
	end
	return true
end

function table.equal(tb1, tb2)
	local kd1, kd2=type(tb1), type(tb2)
	if kd1~=kd2 then return false, 'type not same' .. kd1 .. '~=' .. kd2 end
	if kd1=='table' then return hequal(tb1, tb2) end
	return tb1==tb2 or (tb1~=tb1 and tb2~=tb2), 'value not same' --值判断
end

function table.childequalex( v1, tb2 )
	local has = false
	for k2, v2 in next, tb2 do
		if table.equal( v1, v2 ) then has = true break end
	end
	return has
end

function table.childequal( tb1, tb2 )
	local kd1, kd2=type(tb1), type(tb2)
	if kd1~=kd2 then return false end
	if kd1=='table' then
		for k1, v1 in next, tb1 do
			if not table.childequalex( v1, tb2 ) then return false end
		end
		for k2, v2 in next, tb2 do
			if not table.childequalex( v2, tb1 ) then return false end
		end
		return true
	end
	return tb1==tb2
end

function table.recursive(dest, src)
	local type = type;
	for k, v in next, src do
		if type(v) == "table" and type(dest[k]) == "table" then
			table.recursive(dest[k], v);
		else
			dest[k] = v;
		end
	end
	return dest;
end

function table.sub( tb, from, to )
	from = from or 1
	to = to or #tb
	local ntb = { }
	for ii = from, to do
		table.insert( ntb, tb[ii] )
	end
	return ntb
end

local weakk={__mode='k'}
function table.weakk()
	return setmetatable({}, weakk)
end

local weakv={__mode='v'}
function table.weakv()
	return setmetatable({}, weakv)
end

-- val 在不在 tb里 只检测第一层
function table.intable(tb, val, key)
	for k, v in pairs(tb) do
		if  val == ( key and v[key] or v ) then return k end
	end
end

function table.noarray(s)
	local ss = {}
	for k, v in next, s do
	if type(k) ~= 'number' then ss[k] = v end end
	return ss
end

function table.findpos( tb, val, key )
	for ii = 1, #tb do
		if tb[ii][key] > val then return ii - 1 end
	end
end
function table.ifind(t, v, from, key)
	if v == nil then return nil, nil end
	from = from or 1
	if key then
		for i = from, #t do
			if t[i][key] == v then return i, t[i] end
		end
	else
		for i = from, #t do
			if t[i] == v then return i, t[i] end
		end
	end
	return nil, nil
end


function table.removeValue(tb, val, key)
	local i = table.ifind(tb, val, nil, key)
	if i then table.remove(tb, i) end
end

function table.num(t)
	local n = 0
	for k in next, t do n = n+1 end
	return n
end

function table.isIntTable(tb)
	local keys = table.keys(tb)
	if #keys~=#tb then return false end
	for k, v in next, tb do
		if type(k)~='number' then
			return false
		end
	end
	return true
end