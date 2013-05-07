module(..., package.seeall)

local ShoeReturnerName = nil;

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
local info = GetInfo(enemy.Name, "Marv");
if (info == -1 or info == 2) then 
	me:Say("Who are you?", 1000);
	else
	me:Say("Hi there " .. enemy.Name .. "!", 1000);
	end
me.lookingForFriend=false;
me.reacts_idle = true;
me:Wait(10000);
end

function react_damageTaken()

end

function interact(sinfo, info)
local status = GetInfo(player.Name, "Marv");
if(status == -1) then
	if (info == 0) then
		me:Ask("Hey! I am Marv.\nAnd who would you be?;1;I am "..player.Name.." Slayer of Pizza!;2;None of your business",player);
	elseif (info==1) then
		me:Say("Nice to make your acquaintance",3000, player);
		SetInfo(player.Name, "Marv", 1);
	elseif (info==2) then
		me:Say("Aren't you a grumpy fella",3000, player);
		SetInfo(player.Name, "Marv", 2);
	end
elseif(status == 1) then
	if (info == 0) then
		me:Ask("Hey "..player.Name.." I was wondering\nif you could help me with something;1;Sure, why not?;2;Pizza awaits. Get lost!",player);
	elseif (info == 1) then
		me:Ask("Some big green goop with a crown\nstole my chicken socks.\nCan you get them back for me, please?;3;Sure thing;4;Hell no",player);
	elseif (info==2) then
		me:Say("Fine, I understand. Who would not\n love some delicious pizza",3000,player);
	elseif (info==3) then
		me:Say("Thank you so much!",3000,player);
		SetInfo(player.Name, "Marv", 3);
	elseif (info==4) then
		me:Say("Come back if you change your mind",3000,player);
	end
elseif (status == 2) then
	if (info == 0) then
		me:Ask("Doesn't even tell me his name,\nbut comes back now to talk to me.\nYou are one weird guy;1;Fine... I'm "..player.Name..";2;Because it's none of your business",player);
	elseif (info == 1) then
		SetInfo(player.Name, "Marv", 1);
		me:Say("See that wasn't so bad, was it?",3000,player)
	elseif (info == 2) then
		me:Say("          ...           ",3000,player)
	end
elseif (status==3) then
	me:Say("If only the developer had actually\nmade you able to get back\nmy socks.",3000,player);
end
end