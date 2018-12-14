--local _iowrite = _iowrite
local nowtime = {}
local print = print0 or print
local dumpvisited
local dumpfrom = ''

local function indented( level, ... )
	if PUBLIC then return end
	print(table.concat({ ('  '):rep(level), ...}))
end
local function dumpval( level, name, value, limit )
  local index
  if type(name) == 'number' then
    index = string.format('[%d] = ',name)
  elseif type(name) == 'string'
     and (name == '__VARSLEVEL__' or name == '__ENVIRONMENT__' or name == '__GLOBALS__' or name == '__UPVALUES__' or name == '__LOCALS__') then
    --ignore these, they are debugger generated
    return
  elseif type(name) == 'string' and string.find(name,'^[_%a][_.%w]*$') then
    index = name ..' = '
  else
    index = string.format('[%q] = ',tostring(name))
  end
  if type(value) == 'table' then
    if dumpvisited[value] then
      indented( level, index, string.format('ref%q,',dumpvisited[value]) )
    else
      dumpvisited[value] = tostring(value)
      if (limit or 0) > 0 and level+1 >= limit then
        indented( level, index, dumpvisited[value] )
      else
        indented( level, index, '{  -- ', dumpvisited[value] )
        for n,v in pairs(value) do
          dumpval( level+1, n, v, limit )
        end
		dumpval( level+1, '.meta', getmetatable(value), limit)
        indented( level, '},' )
      end
    end
  else
    if type(value) == 'string' then
      if string.len(value) > 40 then
        indented( level, index, '[[', value, ']];' )
      else
        indented( level, index, string.format('%q',value), ',' )
      end
    else
      indented( level, index, tostring(value), ',' )
    end
  end
end

local function dumpvar( value, limit, name )
  dumpvisited = {}
  dumpval( 0, name or tostring(value), value, limit )
  dumpvisited = nil;
end

debug.dumpdepth = 5
function dump(v,depth)
	if PUBLIC then return end
	local info = debug.getinfo( 2 )
	dumpfrom = info.source .. '|' .. info.currentline --info.linedefined
	nowtime = _time(nowtime, _now())
	Log.sys( 'dump{start', dumpfrom )
  	dumpvar(v,(depth or debug.dumpdepth)+1,tostring(v)..','..dumpfrom)
	Log.sys( 'dump end}', dumpfrom )
end