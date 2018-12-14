--ÄÚ´æÐ¹Â¶¼ì²é
local leakinfo = { }

local stack = { }
local objs = { }

local getupvalues = function( f )
	local ups = { }
	local i = 1
	while true do
		local a, b = debug.getupvalue( f, i )
		if not a then break end
		i = i + 1
		ups[a] = b
	end
	return ups
end

local function finda( t )
	if objs[t] then return end

	objs[t] = true
	if type( t ) == 'function' then t = getupvalues( t ) end

	local count = 0
	for k, v in pairs( t ) do
		count = count + 1
		if tostring( k ):find'finda' ~= 1 and tostring( k ):find'checkleak' ~= 1 then
			if type( k ) == 'table' or type( k ) == 'function' then
				stack[#stack + 1] = '(k:v='..tostring( v )..')'
				finda( k )
				stack[#stack] = nil

				if type( k ) == 'table' then
					local meta = getmetatable( k )
					if meta and type( meta ) == 'table' then
						stack[#stack + 1] = '.meta'
						finda( meta )
						stack[#stack] = nil
					end
				end
			end
			if type( v ) == 'table' or type( v ) == 'function' then
				stack[#stack + 1] = tostring( k )
				finda( v )
				stack[#stack] = nil
				if type( v ) == 'table' then
					local meta = getmetatable( v )
					if meta and type( meta ) == 'table' then
						stack[#stack + 1] = '.meta'
						finda( meta )
						stack[#stack] = nil
					end
				end
			end
		end
	end
	if count > 100 then _stl( '[[[ruo:]]]' .. table.concat( stack, '->' ) .. '::::::' .. count, 'SYS' ) end
end

function _G.checkleak( )
	collectgarbage'collect'
	collectgarbage'collect'
	_stl( 'find_G' )
	finda( _G, '_G' )
	objs = { }
	leakinfo = { }
	stack = { }
end