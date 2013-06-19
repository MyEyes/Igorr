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
me:Say("Hello Ladies", 1000);
me.lookingForFriend=false;
me.reacts_idle = true;
me:Wait(30000);
end

function react_damageTaken()

end

function interact(sinfo, info)
me:ClearState();
me:Move(4300,0.2);
me:Wait(2000);
me:Move(4300,-0.2);
end