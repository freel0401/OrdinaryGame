--print( '!!!!!!!', getmetatable )
local getmetatable = getmetatable

debug.findobj = function( start, comp, root )
	-- init passed
	local passed = { }
	passed[tostring] = true
	passed[debug] = true
	passed[table] = true
	-- TODO: more

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

	local cp = function( t )
		local res = { }
		for k, v in pairs( t ) do res[k] = v end
		return res
	end

	local checktype = { }
	checktype['userdata'] = true
	checktype['table'] = true
	checktype['function'] = true

	local res = { }
	local stack = { }
	local function restrain( obj, name )
		if comp( obj ) then
			stack[#stack + 1] = name
			res[#res + 1] = cp( stack )
			stack[#stack] = nil
			return
		end

		if passed[obj] then return end

		if not obj then
			for i, v in ipairs( res ) do  Log.sys( v ) end
			assert( false )
		end

		passed[obj] = true

		if type( obj ) == 'function' then
			local mt = getupvalues( obj )
			local info = debug.getinfo( obj)
			stack[#stack + 1] = ('%s(function%s|%s)'):format( name ,  info.short_src,info.linedefined )
			--stack[#stack + 1] = ('%s(function)'):format( name )
			restrain( mt, 'upvalues' )
			stack[#stack] = nil
		elseif type( obj ) == 'userdata' then
			local mt = getmetatable( obj )
			if mt then
				stack[#stack + 1] = ('%s(userdata)'):format( name )
				restrain( mt, 'metatables' )
				stack[#stack] = nil
			end
		elseif type( obj ) == 'table' then
			local mt = getmetatable( obj )

			local countk = true
			local countv = true

			if mt then
				stack[#stack + 1] = ('%s(table)%s'):format( name, mt )
				restrain( mt, 'metatable' )
				stack[#stack] = nil

				if mt.__mode then
					if mt.__mode:find( 'k' ) then countk = false end
					if mt.__mode:find( 'v' ) then countv = false end
				end
			end

			for k, v in pairs( obj ) do
				if countk and checktype[type( k )] then
					stack[#stack + 1] = ('%s(table)'):format( name )
					local vt =  getmetatable( k ) and  getmetatable( k ).typestr or ''
					restrain( k, ('key%s|v=%s %s'):format( tostring( k ), vt, tostring( v ) ) )
					stack[#stack] = nil
				end

				if countv and checktype[type( v )] then
					stack[#stack + 1] = ('%s(table)'):format( name )
					if obj[3] and type(obj[3])=='string' and string.sub(obj[3], 1, 6)=='TIMER|' then
						restrain( v, ('val|k=%s'):format( tostring( k )..obj[3] ) )
					else
						local vk =  getmetatable( v ) and getmetatable( v ).typestr or ''
						restrain( v, ('val|k=%s %s'):format( vk, tostring( k ) ) )
					end
					stack[#stack] = nil
				end
			end
		end
	end
	restrain( start, root or 'root' )
	return res
end
