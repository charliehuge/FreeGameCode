------------------------------------------------------------------------
--	Because you never know when you'll need it...
------------------------------------------------------------------------
function IsFibonacci( n )
	-- true if 5 n^2 + 4 or 5n^2 – 4 is square 
	local function isSquare( nn )
		local test = math.floor( math.sqrt( nn ) );
		return test * test == nn;
	end	
	
	return isSquare( 5 * n * n + 4 ) or isSquare( 5 * n * n - 4 );
end