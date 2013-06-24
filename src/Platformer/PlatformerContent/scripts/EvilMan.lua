module(..., package.seeall)

function spawn()
	me.reacts_spotFriendly = true;
	me.lookingForFriend=true
	me.reacts_idle = true;
	me.LookDistance = 32;
end

function spawn_equip()

end

function react_attackReady()

end

function react_idle()
	me.lookingForFriend=true
	me:Wait(2500);
end

function react_spotEnemy()

end

function react_spotFriendly()
	enemy:Stun(6.1);
	me:ClearState();
	me:Move(17, 1);
	me:Move(17, -1);
	me:Lua("me:Say(\"Hmmmmmmmm\", 3000, enemy);", enemy);
	me:Wait(4000);
	me:Lua("me:Say(\"No, this one isn't good either\", 3000, enemy);", enemy);
	me:Wait(2000);
	me:Lua("enemy:Knockback(-1,-100);",enemy);
	me:Wait(2000);
end

function react_damageTaken()

end

function interact(sinfo, info)
end