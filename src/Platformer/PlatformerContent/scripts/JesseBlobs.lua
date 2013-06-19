module(..., package.seeall)

local startPosition;

function spawn()
	me.reacts_spotFriendly = true;
	me.interacts = true;
	me.lookingForFriend=true
	startPosition=me.Position;
end

function spawn_equip()
	me:GivePart(1);
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
	me:Say("Helloooooo Ladies", 1000);
	me.lookingForFriend=false;
	me.reacts_idle = true;
	me:Wait(30000);
end

function react_damageTaken()

end

function interact(sinfo, info)
	local hasPermission = GetInfo(player.Name, "TBPermission");
	if(hasPermission == 1) then
		local diff = me.Position.X-startPosition.x;
		if diff<0.5 and diff>-0.5 then
		me:Say("Let's launch you in the air then",3000,player);
			me:ClearState();
			me:Move(2600,0.2);
			me:JumpCmd();
			me:Move(1400,0.1);
			me:Wait(2000);
			me:Move(3300,-0.2);
		end
	else
		me:Say("I'm sorry, but I can't help you without\nTBs permission.\n\nSend your angry letters to @TotalBiscuit.",3000,player);
	end
end