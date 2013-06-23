module(..., package.seeall)

function spawn()
	me.reacts_spotFriendly = true;
	me.interacts = true;
	me.lookingForFriend=true
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
	me:Say("Hey come over here", 1000);
	me.lookingForFriend=false;
	me.reacts_idle = true;
	me:Wait(30000);
end

function react_damageTaken()

end

function interact(sinfo, info)
	local status = GetInfo(player.Name, "AttackGiver");
	local hasItem = GetInfo(player.Name, "TestItemPickup");
	if(status==-1) then
		if(info==0) then
			me:Ask("I tried to go through the cave in the east.\nBut a scary monster is guarding the exit.\nSo I ran away. But I lost my weapon there I think;1;Testanswer1;2;Testanswer2",player);
		end
	end
end