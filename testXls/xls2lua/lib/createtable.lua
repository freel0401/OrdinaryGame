local weakk=table.weakk()
function _G.ctb(tb)
	if not tb then error('argument error, table expected, but got '..type(tb)); end
	local info=debug.getinfo(2)
	weakk[tb]=info.short_src..info.currentline
	return tb;
end

function _G.exceededtable(num)
	local tb={}
	for k, v in next, weakk do
		tb[v]=tb[v] or 0
		tb[v]=tb[v]+1
	end
	_js('--------------------exceededtable--------------------', num)
	for line, n in next, tb do
		if n>num then
			_js(line)
		end
	end
	_js('--------------------exceededtable--------------------end')
end
