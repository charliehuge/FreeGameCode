--------------------------------------------------------------------
--
-- AgentManager.lua
--
--------------------------------------------------------------------
require "Agent"
require "State"

--------------------------------------------------------------------
-- constants
--------------------------------------------------------------------

--------------------------------------------------------------------
-- members
--------------------------------------------------------------------
local m_Agents = {}

--------------------------------------------------------------------
-- local functions
--------------------------------------------------------------------
local function Init()
	m_Agents = {}
end

--------------------------------------------------------------------
-- global functions
--------------------------------------------------------------------
function AgentManager_AddAgent( templateName )
	table.insert( m_Agents, Agent.new( templateName, "BeAwake" ) )
end

function AgentManager_RemoveAgent( agentid )
	for idx, agent in ipairs( m_Agents ) do
		if( agent.GetID() == agentid ) then
			table.remove( m_Agents, idx )
			return true
		end
	end
	
	-- agent not found
	return false
end

function AgentManager_Update()
	for _, agent in ipairs( m_Agents ) do
		agent.Update()
	end
end

function AgentManager_FindRandomAgent( notThisAgent )
	local rand = math.random( #m_Agents )
	
	if( notThisAgent ) then
		if( #m_Agents <= 1 ) then
			Output( "No one else here!" )
			return nil
		end
		
		while( m_Agents[ rand ] == notThisAgent ) do
			rand = math.random( #m_Agents )
		end
	end
	
	return m_Agents[ rand ]
end

--------------------------------------------------------------------
--------------------------------------------------------------------
Init()