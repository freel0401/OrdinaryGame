function splitaddr( addr )
	local _, _, ip, port, pipe = string.find( addr, "([^:]+):([^,]*)%,(.*)" )
	if ip then
		return ip, toint( port ), pipe
	end
	_, _, ip, port = string.find( addr, "([^:]+):(.*)" )
	_lj( 'splitaddr----------------', ip, port )
	return ip, toint( port )
end

function ip2string(ip)
	local s = string.format('%08x',ip)
	local h1 = toint('0x'..string.sub(s,1,2))
	local h2 = toint('0x'..string.sub(s,3,4))
	local l1 = toint('0x'..string.sub(s,5,6))
	local l2 = toint('0x'..string.sub(s,7,8))
	return string.format('%s.%s.%s.%s',h1,h2,l1,l2)
end

function isip(addr)
	return string.find(addr, "%d+%.%d+%.%d+%.%d+%:%d+");
end

function _G.getIP( ip )
	ip = tonumber( ip )
	return ('%d.%d.%d.%d'):format(_rshift(ip,24), _rshift(ip,16)%256, _rshift(ip,8)%256, ip%256)
end

function str2ip( ip )
	ip = tonumber( ip )
	return ('%d.%d.%d.%d'):format(_rshift(ip,24), _rshift(ip,16)%256, _rshift(ip,8)%256, ip%256)
end

function toip( str )
	local _, _, n1, n2, n3, n4 = string.find( str, "(%d+)%.(%d+)%.(%d+)%.(%d+)" )
	_lj('toip', str, _, _, n1, n2, n3, n4, _lshift(n1, 24) + _lshift(n2, 16) + _lshift(n3, 8) + n4 )
	return _lshift(n1, 24) + _lshift(n2, 16) + _lshift(n3, 8) + n4
end

local hostips
function bindHost( )
	if hostips then return end
	hostips = { }
	local f = io.readall( 'host' )
	if not f then return end
	local lines = string.split( f, "\n" )
	for ii = 1, #lines do
		local line = lines[ii]:trim( )
		if not line:lead'#' then
			local ws = line:split( " " )
			if ws[1] then hostips[ws[1]] = ws[2] end
		end
	end
end

local dnstimeout = 30 * 60 * 1000
local serIp = { }
function host2ip( host )
	if isip( host ) then return host end
	local host, port = splitaddr( host )
	bindHost( )
	if hostips[host] then return hostips[host]..":"..port end
	local tb = serIp[host]
    if not tb or ( _now( ) - tb[2] >= dnstimeout ) then
		local ok, ip = pcall( _hostips, host )
		if not ok then
			serIp[host] = { nil, _now( ) }
		else
			serIp[host] = { getIP( ip ), _now( ) }
		end
	end
	local ip = serIp[host][1]
	if not ip then return end
    host = ( '%s:%s' ):format( ip, port )
	return host
end

function ClearHostCache( )
	serIp = { }
end
_lj('ip<<<<<<<<<<<<<<<<<<<<<<', toip( '10.1.33.238'), toip( '10.1.1.238'), str2ip( 167838190 ), toip( str2ip( 167838190 ) ),  167838190 )