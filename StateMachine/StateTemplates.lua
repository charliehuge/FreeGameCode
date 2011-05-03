--------------------------------------------------------------------
--
-- AgentManager.lua
--
--------------------------------------------------------------------
StateTemplates = {}

--------------------------------------------------------------------
-- GLOBAL
--	Just like opinions and assholes...
--------------------------------------------------------------------
StateTemplates.GLOBAL = {
	OnMessage = function( msg )
		local me = msg.receiver
		
		-- Poke of death!
		if( msg.message == "Delayed Poke of Death!" ) then		
			me.SetAttribute( "fatigue", me.GetAttribute( "fatigue" ) + 10 )
			if( me.GetCurrentStateName() ~= "GoToSleep" ) then
				--me.ChangeState( "GoToSleep" )
				me.Say( "I died!" )
				AgentManager_RemoveAgent( me.GetID() )
			end
		-- Explosion
		elseif( msg.message == "Big 'splosion" ) then
			if( me == msg.sender ) then
				me.RevertToLastState()
			else
				me.Say( "I died in a big explosion! Ow!" )
				AgentManager_RemoveAgent( me.GetID() )
			end
		end
	end,
	Update = function( agent )
		return
	end,
}

--------------------------------------------------------------------
-- BeAwake
--------------------------------------------------------------------
StateTemplates.BeAwake = {
	Update = function( agent )
		--agent.Say("Yep, I'm awake")
		
		if( agent.GetAttribute( "fatigue" ) > 8 ) then
			agent.ChangeState( "GoToSleep" )
			return
		end
		
		-- 10% chance to drink coffee, much like real life
		local rand = math.random()
		if( rand < 0.1 and agent.GetAttribute( "jitters" ) < 5 ) then
			agent.ChangeState( "DrinkCoffee" )
			return
		--[[
		elseif( rand < 0.2 ) then
			agent.ChangeState( "DelayedPokeOfDeath" )
			return
		--]]
		elseif( rand < 0.4 ) then
			agent.ChangeState( "Explode" )
			return
		end
		
		agent.SetAttribute( "fatigue", agent.GetAttribute( "fatigue" ) + 1 )
		agent.SetAttribute( "jitters", agent.GetAttribute( "jitters" ) - 1 )
	end,
}

--------------------------------------------------------------------
-- GoToSleep
--------------------------------------------------------------------
StateTemplates.GoToSleep = {
	Enter = function( agent )
		agent.Say("going to sleep")
	end,
	Update = function( agent )
		--agent.Say("...zzz...")
		
		if( agent.GetAttribute( "fatigue" ) <= 0 ) then
			agent.ChangeState( "BeAwake" )
			return
		end
		
		agent.SetAttribute( "fatigue", agent.GetAttribute( "fatigue" ) - 1 )
		agent.SetAttribute( "jitters", agent.GetAttribute( "jitters" ) - 1 )
	end,
	Exit = function( agent )
		agent.Say("waking up")
	end
}

--------------------------------------------------------------------
-- DrinkCoffee
--------------------------------------------------------------------
StateTemplates.DrinkCoffee = {
	Enter = function( agent )
		agent.Say("downing a shot of coffee")
		agent.SetAttribute( "fatigue", agent.GetAttribute( "fatigue" ) - 1 )
		agent.SetAttribute( "jitters", agent.GetAttribute( "jitters" ) + 5 )
		agent.RevertToLastState()
	end,
}

--------------------------------------------------------------------
-- PokeSomeone
--------------------------------------------------------------------
StateTemplates.PokeSomeone = {
	Enter = function( agent )
		local target = AgentManager_FindRandomAgent( agent )
		agent.Say("poking " .. target.GetName() )
		SendMessage( Message.new( agent, target, "Poke!", GetTime(), nil ) )
		agent.RevertToLastState()
	end,
}

--------------------------------------------------------------------
-- Delayed Poke of Death
--------------------------------------------------------------------
StateTemplates.DelayedPokeOfDeath = {
	Enter = function( agent )
		local target = AgentManager_FindRandomAgent( agent )
		if( target and target.GetCurrentStateName() ~= "GoToSleep" ) then
			agent.Say("administering the delayed poke of death to " .. target.GetName() )
			SendMessage( Message.new( agent, target, "Delayed Poke of Death!", GetTime() + 1000 , nil ) )
		end
		agent.RevertToLastState()
	end,
}

--------------------------------------------------------------------
-- Explode
--------------------------------------------------------------------
StateTemplates.Explode = {
	Enter = function( agent )
		agent.Say( "I'mma chargin' mah explosion!!!" )
		local everyone = AgentManager_FindAllAgents( )
		for _, otherAgent in ipairs( everyone ) do
			SendMessage( Message.new( agent, otherAgent, "Big 'splosion", GetTime() + 2000, nil ) )
		end
	end,
}

--------------------------------------------------------------------
--
--------------------------------------------------------------------
StateTemplates.PickNose = {
	-- TODO
}

--------------------------------------------------------------------
--
--------------------------------------------------------------------
StateTemplates.SurfNet = {
	-- TODO
}