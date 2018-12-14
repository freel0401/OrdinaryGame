function _G.dostring(s)
	return loadstring(s)()
end

--计算随机出的真假结果（单位万分位）
function _G.randomResultMoreAndMore( val )
	if( val <= 0 ) then
		return false
	elseif( val >=10000 ) then
		return true
	end
	local value = math.floor( val )
	local randomNum = math.random( 1, 10000 )
	if( randomNum <= value ) then
		return true
	end
	return false
end

--计算随机出的真假结果（单位千分位）
function _G.randomResultMore( val )
	if( val <= 0 ) then
		return false
	elseif( val >=1000 ) then
		return true
	end
	local value = math.floor( val )
	local randomNum = math.random( 1, 1000 )
	if( randomNum <= value ) then
		return true
	end
	return false
end

--计算随机出的真假结果（单位百分位）
function _G.randomResult( val )
	if( val <= 0 ) then
		return false
	elseif( val >=100 ) then
		return true
	end
	local value = math.floor( val )
	local randomNum = math.random( 1, 100 )
	--print( '----', value, randomNum )
	if( randomNum <= value ) then
		return true
	end
	return false
end

--圆精度距离
local temp = _Vector2.new()
function _G.getDistance(p1, p2)
	temp.x, temp.y  = p1.x - p2.x, p1.y - p2.y
	return temp:magnitude()
end
--圆径精度内
function _G.isInCircle( p1, p2 ,dis, forceV2 )
	return (p1.x - p2.x)^2 + (p1.y - p2.y)^2 <= dis^2
	--return (getDistance( p1, p2, forceV2 ) <= dis)
end
--方精度距离
function _G.getRectDis(p1, p2, forceV2 )
	local max = math.max
	local abs = math.abs
	if p1.z and p2.z and not forceV2 then
		return max(max(abs(p1.x-p2.x),abs(p1.y-p2.y)), abs(p1.z-p2.z))
	else
		return max(abs(p1.x-p2.x),abs(p1.y-p2.y))
	end
end
--方径精度内
function _G.isInRect( p1, p2 ,dis, forceV2 )
	local max = math.max
	local abs = math.abs
	if p1.z and p2.z and not forceV2 then
		return ( abs(p1.x-p2.x)<=dis and abs(p1.y-p2.y)<=dis and abs(p1.z-p2.z)<=dis )
	else
		return ( abs(p1.x-p2.x)<=dis and abs(p1.y-p2.y)<=dis )
	end
end
--点是否在方长方形内
function _G.isInRectArea( p, area )
	local x,y = 0,0
	if area[1].x > area[2].x then x=area[1].x area[1].x = area[2].x area[2].x=x end
	if area[1].y > area[2].y then y=area[1].y area[1].y = area[2].y area[2].y=y end
	return ( p.x >= area[1].x and p.x <= area[2].x ) and ( p.y >= area[1].y and p.y <= area[2].y )
end

--是否场景area中
function _G.isInArea( p, area )
	local x,y = 0, 0
	if area.x1 > area.x2 then x=area.x1 area.x1 = area.x2 area.x2=x end
	if area.y1 > area.y2 then y=area.y1 area.y1 = area.y2 area.y2=y end
	return ( p.x >= area.x1 and p.x <= area.x2 ) and ( p.y >= area.y1 and p.y <= area.y2 )
end

--返回权重随机区间 r=math.weight({20, 30, 50})20%=1,30%=2,50%=3
function math.weight( ratetb )
	if not ratetb then return end
	if #ratetb == 1 then return 1 end

	local ttr = 0
	for _, v in ipairs( ratetb ) do
		ttr = ttr + v
	end
	if ratetb[0] then ttr = ttr + ratetb[0] end
	if ttr == 0 then return 0 end
	local rr = math.random( ttr )

	local count = 0
	for i, v in ipairs( ratetb ) do
		count = count + v
		if count >= rr then
			return i
		end
	end
	return 0
end

-- 从权重组中获得num个随机数
function math.weights( ratetb, num, canrepeat )
	if not canrepeat and #ratetb < num then
		error("ratetb length is less than num")
	end
	local rets = {}
	if canrepeat then
		for i=1, num do
			local r = math.weight( ratetb )
			table.insert(rets, r)
		end
	else
		local cl = table.clone({}, ratetb)
		for i=1, num do
			local r = math.weight( cl )
			local r = math.weight( cl )
			table.insert(rets, r)
			cl[r]=0
		end
	end
	return rets
end

function math.clamp( v, minValue, maxValue )
	if v < minValue then
		return minValue
	end
	if v > maxValue then
		return maxValue
	end
	return v
end

function math.sum( ... )
	local r = 0
	for _, v in ipairs( { ... } ) do
		r = r + v
	end
	return r
end

function math.randoms( _min, _max, rnum )
	assert( _min <= _max, 'math.randoms min:'..tostring(_min)..'>=max:'..tostring(_max) )
	assert( _max - _min + 1 >= rnum, 'math.randoms rnum>_max - _min + 1: min'..tostring(_min)..'max'..tostring(_max)..'rnum'..tostring(rnum) )

	-- local bfind = function( tb, n, start )
		-- local idx = table.binfind( tb, n, start )
		-- return math.abs( idx )
	-- end

	local bfind = function( tb, n, start )
		for ii = start or 1, #tb do
			if tb[ii] > n then
				return ii
			end
		end
		return #tb + 1
	end

	local tb = {}

	for i = 1, rnum do
		local r = math.random( _min, _max - ( i - 1 ) )
		local index = bfind( tb, r, 1 )  -- findpos( tb, r, 1 )
		local nindex = bfind( tb, r + index - 1, index )
		while nindex ~= index do
			index = nindex
			nindex = bfind( tb, r + index - 1, index )
		end
		table.insert( tb, index, r + index - 1 )
	end

	return tb
end

function math.round(n)
	return toint( n, 0.5 )
end

function math.clamp( n, min, max )
	return n < min and n or ( n > max and max or n )
end

-- 通过圆心位置和圆上的点的位置，求该点的切线向量 (mid 圆心坐标, point为圆上点坐标, clockwise为是否顺时针)
-- 顺时针切线向量：sinβ*i - cosβ*j
-- 逆时针切线向量：-sinβ*i + cosβ*j
function _G.tanOfPoint(mid, point, clockwise)
	local vecr = _Vector2.new(point.x-mid.x, point.y-mid.y)
	local angle = math.atan2( vecr.y, vecr.x )
	local newx, newy
	if clockwise then
		newx, newy  = math.sin(angle), -math.cos(angle)
	else
		newx, newy = -math.sin(angle), math.cos(angle)
	end
	return _Vector2.new(newx, newy)
end

-- 判断向量2相对于向量1是否顺时针
function _G.isVecClockWise(vec1, vec2)
	local v = vec1.x*vec2.y - vec2.x*vec1.y
	return v > 0
end

-- 根据点和朝向获取垂直于朝向的向量
function _G.verticalSideDir(rawdir, srcpos, tarpos)
	local tdir = _Vector2.sub(tarpos, srcpos)
	local b = isVecClockWise(tdir, rawdir)
	_Vector2.mul( rawdir, 2, rawdir )
	_Vector2.add( srcpos, rawdir, tarpos )
	local dir = tanOfPoint(srcpos, tarpos, b)
	return dir
end

-- 参数: 角色半径, 怪物坐标, 角色坐标[, 随机角度, 随机距离]
-- 返回值: 怪物追击目标坐标 x, y
function _G.pursuePos(rolerange, monrange, monpos, rolepos, theta, range)
	theta = theta or PURSUETHETA
	range = range or PURSYERANGE
	local theta = math.random(-theta*10000, theta*10000)
	theta = theta / 10000
	local d = math.random(rolerange + monrange, rolerange + monrange + range)
	local dx = monpos.x - rolepos.x
	local dy = monpos.y - rolepos.y
	local costheta = math.cos(theta)
	local sintheta = math.sin(theta)
	local dxdxdydy = ((dx * dx + dy * dy) ^ 0.5)
	local x = (dx * costheta + dy * sintheta) * d / dxdxdydy + rolepos.x
	local y = (dy * costheta - dx * sintheta) * d / dxdxdydy + rolepos.y
	return {x = x, y = y }
end

local uuidseed = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' }
local tempuuid = {}
-- useline == 1 xxxxxxxx-xxxx- xxxx-xxxxxxxxxxxxxxxx(8-4-4-16)
-- useline == 2 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx (8-4-4-4-12)
function _G.getUUID(useline)
	table.clear(tempuuid)
	for i = 1, 32 do
		if (useline==1 and (i==8 or i==12 or i==16 or i==32)) or (useline==2 and (i==8 or i==12 or i==16 or i==20 or i==32)) then
			tempuuid[#tempuuid+1] = '-'
		end
		tempuuid[#tempuuid+1] = uuidseed[math.random(1, 16)]
	end
	return table.concat(tempuuid)
end