function _G._T(...)
	return ...
end

function string.MD5(s) --大写md5
	return string.upper(s:md5())
end

function string.tohex( c )
	local i = string.byte( c )
	return string.format( "%%%X", i )
end

function string.split0(str, pattern)
	pattern = pattern or "[^%s]+"
	if pattern == ',' then pattern = '[^,%s+]' end
	if pattern:len( ) == 0 then pattern = "[^%s]+" end
	local parts = {__index = table.insert}
	setmetatable(parts, parts)
	str:gsub(pattern, parts)
	setmetatable(parts, nil)
	parts.__index = nil
	return parts
end

function string.split(s, delimiter)--指定字符分割如str:split(',')
	if ''==s then return {} end
	local t, i, j, k = {}, 1, 1, 1
	while i <= #s+1 do
		j, k = s:find(delimiter, i)
		j, k = j or #s+1, k or #s+1
		t[#t+1] = s:sub(i, j-1)
		i = k + 1
	end
	return t
end

function string.uchar(u)
	if u <= 127 then return string.char(u) end
	if u <= 0x7ff then return string.char(0xc0+toint(u/64), 0x80+u%64) end
	return string.char(0xe0+toint(u/4096), 0x80+toint(u/64%64), 0x80+u%64)
end

function string.slen(s)	--视觉字宽：以半角为1，全角为2算总宽
	local chars = string.ulen(s)	--字符数
	local bytes = string.len(s)		--字节数
	local utfch = (bytes-chars)/2	--UTF字数
	return chars + utfch			--串宽
end

function string.trim(s)--?
	return (s:gsub('^%s*(.-)%s*$', '%1'))
end

function string.ip2string(ip)--ip int->xxx.xxx.xxx.xxx
	if type(ip) == 'string' then return ip end
	local s = string.format('%08x',ip)
	local h1 = toint('0x'..string.sub(s,1,2))
	local h2 = toint('0x'..string.sub(s,3,4))
	local l1 = toint('0x'..string.sub(s,5,6))
	local l2 = toint('0x'..string.sub(s,7,8))
	return string.format('%s.%s.%s.%s',h1,h2,l1,l2)
end

function Text(str, ...)
	local n = select('#',...)
	if n==0 then return str end
	local a, args = ..., {...}
	return (str:gsub('{(%%?)(#?)([^{%@.}]+)(%@?)}', function (p, i, k, un)
		k = toint(k) or k
		if i == '#' then
			k = k<=n and tostring(args[k]) or nil
		else
			k = a and a[k]~=nil and tostring(a[k]) or nil
		end
		if un == '@' then k = k and k:lead('[') and k:sub(8) or k end
		return k and p=='%' and ('%q'):format(k) or k
	end))
end
string.text = Text


--起名合法性
local ss = string.split('21,7e,b7,b7,4e00,9fa5,ff01,ff5e',',' )
for i, t in inext, ss do
	ss[i] = assert(tonumber(t, 16), 'invalid unicode {#1}', t)
end
string._cmisc = ss
function string.checkname( s, len )
	len = len or 6
	if s==''	then return false end
	if string.ulen(s)>len then return false,_T'非法角色名长' end
	if string.find(s,'%p+')then return false end
	if string.find(s,'%c+')then return false end

	if platswitch and platswitch.ignoreUCSCheck then return true end

	if string.find(s,' ')then return false end
	for i, c in inext, { s:ucs(1,-1,'c') } do
		local k = table.binfind(string._cmisc, c)
		if not (k > 0 or k%2 == 0) then return false end
	end
	return true
end

_G.mstr = function( t )
	local key = string.gsub( t[1], '%$(%d+)', function( s )
		local index = toint(s)
		local v = index and t[index+1]
		return v and tostring( v ):gsub( "%%", "%1" ) or s
	end )
	local r = string.gsub( key, '<<<([^<^>]+)>>>', function( s )
		local v = t[s]
		if v == nil then return table.concat( { '<<<', s, ">>>" } ) end
		return tostring( v ):gsub( "%%", "%1" )
	end)
	return r
end