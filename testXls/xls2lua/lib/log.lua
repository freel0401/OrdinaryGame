_G.Log = { }
_G.NewLogPlatFrom = os.info.platform == 'androidxianqian'
os.info = os.info or { }

local print0
local logfile --上次写log的文件
local lastlogpath --上次写log的文件路径

local t0 = os.now( )
local tail = nil
local timetb = { }
local function print0( ... )
	if not tail then
		tail = _time( timetb, t0 )
		tail = ( '%02d%02d' ):format( tail.min, tail.sec )
	end
	timetb = _time( timetb, os.now( ) )
	local logpath
	if os.info.line_id then
		logpath = "logs/"..string.format("%04d%02d%02d%02d%s_%s_%s_%05d_%05d.log", timetb.year, timetb.month, timetb.day, timetb.hour, tail, os.info.servertype, os.info.logic or'', os.info.line_id or 0, os.info.server_id or 0 )
	else
		logpath = "logs/"..string.format("%04d%02d%02d%02d%s_%s_%05d.log", timetb.year, timetb.month, timetb.day, timetb.hour, tail, os.info.servertype, os.info.server_id or 0 )
	end
	if logpath ~= lastlogpath then
		if logfile then
			logfile:close()
		else
			io.close( )
		end
		logfile = io.open( logpath, "wb" )
		logfile:setvbuf'line'
		io.stdout = logfile
		io.output( logfile )
		lastlogpath = logpath
	end
	logfile:write( table.concat( {...}, ' ' )..'\n'  )
end
if not LOGFILE then print0 = print end
local doprint = function( head, t, console, file )
	local s = { '[', head, ']' }

	if LOGHEAD then
		s[#s + 1] = ( 'S%05dL%05d %s ' ):format( os.info.server_id or 0, os.info.line_id or 0, timehelper( _now( 0 ), 0 ).getstr( ) )
	end

	local len=table.maxn(t)
	for i = 1, len do
		local v=t[i]
		table.push(s, tostring(v), ',')
	end
	if len>=1 then table.remove(s); end
	print0(table.concat(s))
end

Log.sys = function( ... )
	--local s = debug.getinfo( 2, 'S' )
	--doprint( 'sys', { table.concat{ '@', s.short_src, '@', s.linedefined}, ... }, true, true )
	doprint( 'sys', { ... }, true, true )
end

Log.debug = function( ... )
	if PUBLIC then return end
	doprint( 'debug', { ... }, true, false )
end

Log.warn = function( ... )
	doprint( 'warn', { ... }, true, true )
end

Log.fatal = function( ... )
	doprint( 'fatal', { ... }, true, true )
	if LogHelper then LogHelper.add('fatal',...) end
end

Log.tick = function( ... )
	if not SHOWTICK then return end
	doprint( 'tick', { ... }, true, true )
end

Log.writeLog = function( str, file, date, filename )
	if not file then
		date = _now( 86400 )
		local t = _time( timetb, date, 86400 )
		Log.sys( 'file', t.year, t.month, t.day )
		local res, ret = pcall( io.open, filename, 'a+b' )
		if res then
			file = ret
			file:setvbuf'line'
		else Log.fatal( 'file open failed', ret, str ) end
	end

	if _now( 86400 ) ~= date then
		if file then file:close( ) end
		file = nil
		date = _now( 86400 )
		local t = _time( timetb, date, 86400 )
		Log.sys( 'file', t.year, t.month, t.day )
		local res, ret = pcall( io.open, filename, 'a+b' )
		if res then
			file = ret
			file:setvbuf'line'
		else Log.fatal( 'file open failed', ret, str ) end
	end
	if file then file:write( str .. '\n' ) end
	return file, date
end

-- 西游log（分开的）
_G.xyfile = {
	RoleLevelUp	= { },
	ChatLog		= { },
}

Log.xy = function( interface, str )
	-- if not PUBLIC then return end
	-- if NewLogPlatFrom then
		-- local t = _time( timetb, _now( 86400 ), 86400 )
		-- local filename = ( '/data/logs/xiyou_log/mfsy/mobile_' .. interface .. '_%04d%02d%02d.txt' ):format( t.year, t.month, t.day )
		-- xyfile[interface].file, xyfile[interface].date = Log.writeLog( str, xyfile[interface].file, xyfile[interface].date, filename )
	-- end
end

local accfile = nil
local accdate = nil
Log.account = function( interface, str )
	-- if not PUBLIC then return end
	local t = _time( timetb, _now( 86400 ), 86400 )
	local filename = ( 'data/account_log/mobile_' .. interface .. '_%04d%02d%02d.txt' ):format( t.year, t.month, t.day )
	accfile, accdate = Log.writeLog( str, accfile, accdate, filename )
end

local cyfile = nil
local cydate = nil
Log.cy = function( str )
	local t = _time( timetb, _now( 86400 ), 86400 )
	local filename = ( 'data/%04d-%02d-%02d.log' ):format( t.year, t.month, t.day )
	cyfile, cydate = Log.writeLog( str, cyfile, cydate, filename )
end

local defile = nil
local dedate = nil
Log.bebug = function( str )
	local t = _time( timetb, _now( 86400 ), 86400 )
	local filename = ( 'data/debug_%04d-%02d-%02d.log' ):format( t.year, t.month, t.day )
	defile, dedate = Log.writeLog( str, defile, dedate, filename )
end

local programmers = { 'lj', 'js', 'gx', 'lzl', 'zkx', 'zwg', 'zss', 'wl', 'stl', 'wyh','ct', 'ls', 'ak', 'ws', 'dc', 'qz' }
for i, v in ipairs( programmers ) do
	_G['_'..v] = function( ... )
		if PUBLIC then return end
		local s = debug.getinfo( 2, 'S' )
		local s0 = { }
		s0[#s0 + 1] = v
		s0[#s0 + 1] = '@'
		s0[#s0 + 1] = s.short_src
		s0[#s0 + 1] = '@'
		s0[#s0 + 1] = s.linedefined
		s = table.concat( s0 )
		doprint( s, { ... }, true, true )
	end
end

-- if not PUBLIC then
-- 	_G.luaprint = print
-- 	_G.print = function( ) assert( false ) end
-- end