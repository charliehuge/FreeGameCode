--------------------------------------------------------------------
--
-- State.lua
--
--------------------------------------------------------------------
require "StateTemplates"

State = {}
function State.new( name )
	local self = {}
	
	---------------------------------------------------------------
	-- Data Members
	---------------------------------------------------------------
	local m_Name = name
	
	---------------------------------------------------------------
	-- Public Functions
	---------------------------------------------------------------		
	function self.Enter( agent )
		--Output( "Entering " .. m_Name .. " for " .. agent.GetName() )
		if( StateTemplates[ m_Name ] and StateTemplates[ m_Name ].Enter ) then
			StateTemplates[ m_Name ].Enter( agent );
		end
	end
	
	function self.Update( agent )
		--Output( "Updating " .. m_Name .. " for " .. agent.GetName() )
		if( StateTemplates[ m_Name ] and StateTemplates[ m_Name ].Update ) then
			StateTemplates[ m_Name ].Update( agent );
		end
	end
	
	function self.Exit( agent )
		--Output( "Exiting " .. m_Name .. " for " .. agent.GetName() )
		if( StateTemplates[ m_Name ] and StateTemplates[ m_Name ].Exit ) then
			StateTemplates[ m_Name ].Exit( agent );
		end
	end
	
	function self.OnMessage( msg )
		if( StateTemplates[ m_Name ] and StateTemplates[ m_Name ].OnMessage ) then
			StateTemplates[ m_Name ].OnMessage( msg )
		end
	end
	
	function self.GetName()
		return m_Name
	end
	
	---------------------------------------------------------------
	return self
end