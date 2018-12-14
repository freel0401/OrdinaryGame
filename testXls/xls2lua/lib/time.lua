--时间/日期相关
function _G.Cd(e,k,t, readonly) --自定义cd中
	local now = os.now()
	if not e.___cooldown then e.___cooldown = {} end
	if e.___cooldown[k] and now-e.___cooldown[k]<t then
		return true, t-(now-e.___cooldown[k])
	end
	if not readonly then
		e.___cooldown[k] = now
	end
end

local d1,d2 = {},{}
function _G.sameDay(t1,t2)--两个ms时间是否同日

	_time(d1,t1)
	_time(d2,t2)
	return d1.year==d2.year and d1.month==d2.month and d1.day==d2.day
end
function _G.sameWeek(t1,t2)--两个ms时间是否同周
	_time(d1,t1)
	_time(d2,t2)
	local w1 = d1.wday
	local w2 = d2.wday
	if w1==0 then w1=7 end
	if w2==0 then w2=7 end
	_time(d1,t1-w1*86400000)
	_time(d2,t2-w2*86400000)
	return d1.year==d2.year and d1.month==d2.month and d1.day==d2.day
end
function _G.sameMonth(t1,t2)--两个ms时间是否同月
	_time(d1,t1)
	_time(d2,t2)
	return d1.year==d2.year and d1.month==d2.month
end

function _G.day0time(t)--取得当指定时间的0点时间
	return math.floor((t or os.now())/86400000)*86400000--今日0点
end
local tmpd = {}
function _G.getDayKeyNum(t)--return YYYYMMDD
	local now = t or _now()
	local d = _time(tmpd,now)
	local dk = d.year*10000+d.month*100+d.day
	return dk
end

function _G.formatDate(str, time)
	_time(tmpd, time or _now())
	return mstr{str, year=tmpd.year, month=tmpd.month, day=tmpd.day, hour=tmpd.hour, min=tmpd.min, sec=tmpd.sec, msec=tmpd.msec, usec=tmpd.usec}
end

local __1970 = _time(1, { year = 1970, month = 1, day =1, hour = 0, min = 0, sec = 0 }) --= -946684800
function os.time( date ) --取(date or 当前地间)的unix时间 sec
	if date then
		return _time(1, date) - __1970
	else
		return os.utc(1) - __1970
	end
end

--两个时间相差的天数--单位毫秒
function _G.day2day(d1,d2)
	d1 = day0time( d1 )
	return math.ceil( (d2 - d1)/86400000 )
end