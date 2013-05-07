module(..., package.seeall)

function spawn()
me.interacts=true
me.reacts_spotFriendly=true
me.lookingForFriend=true
end

function spawn_equip()
end

function react_attackReady()
end

function react_idle()
me.lookingForFriend=true
me.reacts_idle=false
end

function react_spotEnemy()
end

function react_spotFriendly()
flag = GetInfo(enemy.Name,"TalksToChickens");
if(flag > -1) then
  return end
me.lookingForFriend=false
me:Wait(15000)
me.reacts_idle=true
me:Say("Come and talk to me", 2000, enemy)
end

function react_damageTaken()
end

function interact(sinfo, info)
	flag = GetInfo(player.Name,"TalksToChickens");
	if(flag == -1) then
		SetInfo(player.Name,"TalksToChickens", 0);
		me:Ask("Do you always talk\nto Chickens?;0;Yes;0;Mmmhh Chicken", player);
	elseif(info == 0 and flag == 0) then
		me:Ask("Do you see this Question?;1;Yes!;2;No :(;3;What can you do?", player);
	elseif (info==1) then
		me:Say("Great!", 2000, player);
	elseif (info==2) then
		me:Say("Oh no!", 2000, player);
	elseif (info==3) then
		me:Ask("Hello!\nI can lay eggs.\nWith those eggs you\ncan blow people up!;4;And then?", player);
	elseif (info==4) then
		me:Say("You can now throw eggs!\nGive it a try.", 3500, player);
		player:GivePart(80);
	end
end