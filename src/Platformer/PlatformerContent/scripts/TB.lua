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
	me:Say("Why did I even hide here...", 1000);
	me.lookingForFriend=false;
	me.reacts_idle = true;
	me:Wait(30000);
end

function react_damageTaken()

end

function interact(sinfo, info)
	local status = GetInfo(player.Name, "TBPermission");
	if(status == -1) then
		if (info == 0) then
			me:Ask("What do you want?;1;Jesse said I need your permission;2;Forget about it.",player);
		elseif (info==1) then
			me:Say("Fine, but it will all be his fault.\nGo and tell him.",3000, player);
			SetInfo(player.Name, "TBPermission", 1);
		elseif (info==2) then
			me:Say("Waste of time...",3000, player);
		end
	elseif(status == 1) then
		me:Say("Hush... Talk to Jesse",3000,player);
	end
end