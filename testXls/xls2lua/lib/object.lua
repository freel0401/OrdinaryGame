--swfclass = {};
local cs = {};
function _class( child, base, ... )
--	_.s( child );
	if( cs[ child ] ) then error( "duplicate class : " .. child ) end;
	local c = {}
	_G[child]=c;
	table.append( c, base or {} );
	for ii = 1, select("#", ...) do
		table.append( c, select(ii, ...) )
	end
	if base then setmetatable( c, base ) end;
	c.__index = c;
	c._className = child;
	cs[ child ] = c;
--	swfclass[child] = c;
end

_class( "Object", nil )
function Object.new ( self )
	local instance = { }
	setmetatable( instance , self )
	instance._className = self._className
	return instance
end

function Object.instanceof ( self, class )
	if class == nil then return false end;
	local c = self;
	while c do
		if( c == class ) then return true end;
		c = getmetatable( c );
	end
	return false;
end

function Object.toString( self )
	return table.tostring( self );
end

function Object.__tostring(self)
	return self._className;
end

function Object.load( self ) --userdata 无法恢复
	local super = _G[ self._className ];
	if not super then return self; end
	table.copyAll( self, super )
	return self;
end
