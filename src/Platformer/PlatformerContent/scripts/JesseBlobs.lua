module(..., package.seeall)

local startPosition;

function spawn()
me.reacts_spotFriendly = true;
me.interacts = true;
me.lookingForFriend=true
startPosition=me.Position;
end

function spawn_equip()

end

function react_attackReady()

end

function react_idle()
me.lookingForFriend=true
me.reacts_idle=false;
end

function react_spotEnemy()

end

function react_spotFriendly()
me:Say("Hello Ladies", 1000);
me.lookingForFriend=false;
me.reacts_idle = true;
me:Wait(30000);
end

function react_damageTaken()

end

function interact(sinfo, info)
local diff = me.Position.X-startPosition.x;
if diff<0.5 and diff>-0.5 then
	me:ClearState();
	me:Move(4300,0.2);
	me:Wait(2000);
	me:Move(4300,-0.2);
end
end