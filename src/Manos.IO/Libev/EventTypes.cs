using System;

namespace Libev
{
	[Flags]
	enum EventTypes
	{
		Undefined   = -1,
		None        = 0x00,
		Read        = 0x01,
		Write       = 0x02,
		Timeout     = 0x00000100,
		Periodic    = 0x00000200,
		Signal      = 0x00000400,
	}
}

