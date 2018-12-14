local olderror = error
function error( a, ... )
	olderror( a or 'error nil or false!', ... )
end

function assert(ok, format, ...)
	if not ok then olderror(format==nil and 'assertion failed!'
		or select('#', ...)>0 and string.text(format, ...) or format, 2) end
	return ok
end
