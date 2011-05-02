--------------------------------------------------------------------
--
-- Agent.lua
--
-- base class for anything that can act and have states
--
--------------------------------------------------------------------

local nextValidID = 0

local function GetNextValidID()
	nextValidID = nextValidID + 1
	return nextValidID
end

Agent = {}
function Agent.new( name, initialState )
	local self = {}
	
	---------------------------------------------------------------
	-- Data Members
	
	local m_ID 				= GetNextValidID()
	local m_Name 			= name
	local m_GlobalState		= State.new( "GLOBAL" )
	local m_CurrentState 	= State.new( initialState )
	local m_LastState		= nil
	local m_Attributes = {
		fatigue		= 0,
		jitters		= 0,
		bladder		= 0,
		hunger		= 0,
		thirst		= 0,
		frustration = 0,
		depression 	= 0,
	}
	
	---------------------------------------------------------------
	-- Private Functions
	
	
	---------------------------------------------------------------
	-- Public Functions
	
	function self.Update()
		--m_GlobalState.Update( self )
		m_CurrentState.Update( self )
	end
	
	function self.GetCurrentStateName()
		return m_CurrentState.GetName()
	end
	
	function self.ChangeState( newState )
		m_CurrentState.Exit( self )
		m_LastState = State.new( m_CurrentState.GetName() )
		m_CurrentState = State.new( newState )
		m_CurrentState.Enter( self )
	end
	
	function self.RevertToLastState()
		self.ChangeState( m_LastState.GetName() )
	end
	
	function self.GetName()
		return m_Name
	end
	
	function self.GetID()
		return m_ID
	end
	
	function self.SetAttribute( attributeName, value )
		m_Attributes[ attributeName ] = value
	end
	
	function self.GetAttribute( attributeName )
		return m_Attributes[ attributeName ]
	end
	
	function self.Say( text )
		Output( m_Name .. ": " .. text )
	end
	
	function self.HandleMessage( message )
		m_GlobalState.OnMessage( message )
		m_CurrentState.OnMessage( message )
	end
	
	---------------------------------------------------------------
	return self
end