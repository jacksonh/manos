// (wrapper runtime-invoke) object:runtime_invoke_void__this___object (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.1   
IL_001c: ldind.ref 
IL_001d: ldarg.3   
IL_001e: calli     <object>
IL_0023: ldnull    
IL_0024: stloc.0   
IL_0025: leave     IL_0043
IL_002a: pop       
IL_002b: ldarg.2   
IL_002c: ldc.i4.0  
IL_002d: cgt.un    
IL_002f: endfilter 
IL_0031: stloc.1   
IL_0032: ldarg.2   
IL_0033: ldloc.1   
IL_0034: stind.ref 
IL_0035: ldnull    
IL_0036: stloc.0   
IL_0037: ldloc.1   
IL_0038: mono_icall <runtime_invoke_reset_abort>
IL_003e: leave     IL_0043
IL_0043: ldloc.0   
IL_0044: ret       
// (wrapper runtime-invoke) object:runtime_invoke_void (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.3   
IL_001b: calli     <>
IL_0020: ldnull    
IL_0021: stloc.0   
IL_0022: leave     IL_0040
IL_0027: pop       
IL_0028: ldarg.2   
IL_0029: ldc.i4.0  
IL_002a: cgt.un    
IL_002c: endfilter 
IL_002e: stloc.1   
IL_002f: ldarg.2   
IL_0030: ldloc.1   
IL_0031: stind.ref 
IL_0032: ldnull    
IL_0033: stloc.0   
IL_0034: ldloc.1   
IL_0035: mono_icall <runtime_invoke_reset_abort>
IL_003b: leave     IL_0040
IL_0040: ldloc.0   
IL_0041: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_array_new_specific (intptr,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,int>
IL_000d: ret       
// (wrapper stelemref) object:stelemref (object,intptr,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldelema   <Object>
IL_0007: stloc.2   
IL_0008: ldarg.2   
IL_0009: brfalse   IL_004f
IL_000e: ldarg.0   
IL_000f: mono_objaddr
IL_0011: ldind.i   
IL_0012: mono_objaddr
IL_0014: ldind.i   
IL_0015: mono_objaddr
IL_0017: ldind.i   
IL_0018: stloc.0   
IL_0019: ldarg.2   
IL_001a: mono_objaddr
IL_001c: ldind.i   
IL_001d: mono_objaddr
IL_001f: ldind.i   
IL_0020: stloc.1   
IL_0021: ldloc.1   
IL_0022: mono_objaddr
IL_0024: ldc.i4.s  24
IL_0026: add       
IL_0027: ldind.u2  
IL_0028: ldloc.0   
IL_0029: mono_objaddr
IL_002b: ldc.i4.s  24
IL_002d: add       
IL_002e: ldind.u2  
IL_002f: blt.un    IL_0053
IL_0034: ldloc.1   
IL_0035: mono_objaddr
IL_0037: ldc.i4.s  16
IL_0039: add       
IL_003a: ldind.i   
IL_003b: ldloc.0   
IL_003c: mono_objaddr
IL_003e: ldc.i4.s  24
IL_0040: add       
IL_0041: ldind.u2  
IL_0042: ldc.i4.1  
IL_0043: sub       
IL_0044: ldc.i4.s  8
IL_0046: mul       
IL_0047: add       
IL_0048: ldind.i   
IL_0049: ldloc.0   
IL_004a: bne.un    IL_0053
IL_004f: ldloc.2   
IL_0050: ldarg.2   
IL_0051: stind.ref 
IL_0052: ret       
IL_0053: ldarg.2   
IL_0054: ldloc.0   
IL_0055: mono_icall <mono_object_isinst>
IL_005b: brtrue    IL_004f
IL_0060: newobj    <System.ArrayTypeMismatchException:.ctor ()>
IL_0065: throw     
IL_0066: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_object_new_fast (intptr)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <intptr>
IL_000c: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_VolumeSeparatorChar ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_DirectorySeparatorChar ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_AltDirectorySeparatorChar ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_PathSeparator ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) char:GetDataTablePointers (byte*&,byte*&,double*&,uint16*&,uint16*&,uint16*&,uint16*&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.s   5
IL_0008: ldarg.s   6
IL_000a: mono_ldptr2
IL_0010: calli     <byte*&,byte*&,double*&,uint16*&,uint16*&,uint16*&,uint16*&>
IL_0015: ret       
// (wrapper managed-to-managed) string:.ctor (char,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: call      <string:CreateString (char,int)>
IL_0008: ret       
// (wrapper alloc) object:Alloc (intptr,int)
IL_0000: ldarg.1   
IL_0001: ldc.i4.1  
IL_0002: add       
IL_0003: ldc.i4.1  
IL_0004: shl       
IL_0005: ldc.i4.s  20
IL_0007: add       
IL_0008: stloc.0   
IL_0009: ldloc.0   
IL_000a: ldc.i4    512
IL_000f: bgt.un.s  IL_0051
IL_0011: ldloc.0   
IL_0012: ldc.i4.s  24
IL_0014: ble.un.s  IL_0051
IL_0016: ldloc.0   
IL_0017: ldc.i4.7  
IL_0018: add       
IL_0019: ldc.i4.3  
IL_001a: shr.un    
IL_001b: ldc.i4.3  
IL_001c: shl       
IL_001d: stloc.1   
IL_001e: mono_tls  -8
IL_0024: ldc.i4.s  64
IL_0026: add       
IL_0027: ldloc.1   
IL_0028: add       
IL_0029: stloc.2   
IL_002a: ldloc.2   
IL_002b: ldind.i   
IL_002c: stloc.3   
IL_002d: ldloc.3   
IL_002e: ldc.i4    4096
IL_0033: blt.un.s  IL_0051
IL_0035: ldloc.2   
IL_0036: ldloc.3   
IL_0037: ldind.i   
IL_0038: stind.i   
IL_0039: ldloc.3   
IL_003a: ldarg.0   
IL_003b: stind.i   
IL_003c: ldloc.3   
IL_003d: ldc.i4.s  8
IL_003f: add       
IL_0040: ldc.i4.0  
IL_0041: stind.i   
IL_0042: ldloc.3   
IL_0043: ldc.i4.s  16
IL_0045: add       
IL_0046: ldarg.1   
IL_0047: stind.i4  
IL_0048: ldloc.3   
IL_0049: ldloc.0   
IL_004a: ldc.i4.2  
IL_004b: sub       
IL_004c: add       
IL_004d: ldc.i4.0  
IL_004e: stind.i2  
IL_004f: ldloc.3   
IL_0050: ret       
IL_0051: ldarg.1   
IL_0052: mono_icall <mono_string_alloc>
IL_0058: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_string_alloc (intptr)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <intptr>
IL_000c: ret       
// (wrapper managed-to-native) System.MonoType:get_Assembly ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Security.SecurityManager:get_SecurityEnabled ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper alloc) object:Alloc (intptr)
IL_0000: ldarg.0   
IL_0001: ldc.i4.0  
IL_0002: add       
IL_0003: ldind.i   
IL_0004: ldc.i4.s  28
IL_0006: add       
IL_0007: ldind.u4  
IL_0008: stloc.0   
IL_0009: ldloc.0   
IL_000a: ldc.i4.7  
IL_000b: add       
IL_000c: ldc.i4.3  
IL_000d: shr.un    
IL_000e: ldc.i4.3  
IL_000f: shl       
IL_0010: stloc.1   
IL_0011: mono_tls  -8
IL_0017: ldc.i4.s  64
IL_0019: add       
IL_001a: ldloc.1   
IL_001b: add       
IL_001c: stloc.2   
IL_001d: ldloc.2   
IL_001e: ldind.i   
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: ldc.i4    4096
IL_0026: blt.un.s  IL_004d
IL_0028: ldloc.2   
IL_0029: ldloc.3   
IL_002a: ldind.i   
IL_002b: stind.i   
IL_002c: ldloc.3   
IL_002d: ldarg.0   
IL_002e: stind.i   
IL_002f: ldloc.3   
IL_0030: ldloc.0   
IL_0031: add       
IL_0032: stloc.s   5
IL_0034: ldloc.3   
IL_0035: ldc.i4.s  8
IL_0037: add       
IL_0038: stloc.s   4
IL_003a: ldloc.s   4
IL_003c: ldc.i4.0  
IL_003d: stind.i   
IL_003e: ldloc.s   4
IL_0040: ldc.i4.s  8
IL_0042: add       
IL_0043: stloc.s   4
IL_0045: ldloc.s   4
IL_0047: ldloc.s   5
IL_0049: blt.un.s  IL_003a
IL_004b: ldloc.3   
IL_004c: ret       
IL_004d: ldarg.0   
IL_004e: mono_icall <mono_object_new_specific>
IL_0054: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_object_new_specific (intptr)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <intptr>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:FillName (System.Reflection.Assembly,System.Reflection.AssemblyName)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <Assembly,AssemblyName>
IL_000d: ret       
// (wrapper runtime-invoke) object:runtime_invoke_void__this___int_int_int_int (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.1   
IL_001c: ldind.i   
IL_001d: ldind.i4  
IL_001e: ldarg.1   
IL_001f: ldc.i4.s  8
IL_0021: add       
IL_0022: ldind.i   
IL_0023: ldind.i4  
IL_0024: ldarg.1   
IL_0025: ldc.i4.s  16
IL_0027: add       
IL_0028: ldind.i   
IL_0029: ldind.i4  
IL_002a: ldarg.1   
IL_002b: ldc.i4.s  24
IL_002d: add       
IL_002e: ldind.i   
IL_002f: ldind.i4  
IL_0030: ldarg.3   
IL_0031: calli     <int,int,int,int>
IL_0036: ldnull    
IL_0037: stloc.0   
IL_0038: leave     IL_0056
IL_003d: pop       
IL_003e: ldarg.2   
IL_003f: ldc.i4.0  
IL_0040: cgt.un    
IL_0042: endfilter 
IL_0044: stloc.1   
IL_0045: ldarg.2   
IL_0046: ldloc.1   
IL_0047: stind.ref 
IL_0048: ldnull    
IL_0049: stloc.0   
IL_004a: ldloc.1   
IL_004b: mono_icall <runtime_invoke_reset_abort>
IL_0051: leave     IL_0056
IL_0056: ldloc.0   
IL_0057: ret       
// (wrapper managed-to-native) object:MemberwiseClone ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper runtime-invoke) object:runtime_invoke_object_object_sbyte (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.1   
IL_001b: ldind.ref 
IL_001c: ldarg.1   
IL_001d: ldc.i4.s  8
IL_001f: add       
IL_0020: ldind.i   
IL_0021: ldind.u1  
IL_0022: ldarg.3   
IL_0023: calli     <object,sbyte>
IL_0028: stloc.0   
IL_0029: leave     IL_0047
IL_002e: pop       
IL_002f: ldarg.2   
IL_0030: ldc.i4.0  
IL_0031: cgt.un    
IL_0033: endfilter 
IL_0035: stloc.1   
IL_0036: ldarg.2   
IL_0037: ldloc.1   
IL_0038: stind.ref 
IL_0039: ldnull    
IL_003a: stloc.0   
IL_003b: ldloc.1   
IL_003c: mono_icall <runtime_invoke_reset_abort>
IL_0042: leave     IL_0047
IL_0047: ldloc.0   
IL_0048: ret       
// (wrapper managed-to-native) System.Array:FastCopy (System.Array,int,System.Array,int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: mono_ldptr2
IL_000c: calli     <Array,int,Array,int,int>
IL_0011: ret       
// (wrapper managed-to-native) System.Threading.Thread:GetCachedCurrentCulture ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Threading.Thread:GetSerializedCurrentCulture ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Globalization.CultureInfo:construct_internal_locale_from_current_locale (System.Globalization.CultureInfo)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <CultureInfo>
IL_000c: ret       
// (wrapper managed-to-native) System.Threading.Thread:SetCachedCurrentCulture (System.Globalization.CultureInfo)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <CultureInfo>
IL_0019: ret       
// (wrapper managed-to-native) System.NumberFormatter:GetFormatterTables (ulong*&,int*&,char*&,char*&,long*&,int*&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.s   5
IL_0008: mono_ldptr2
IL_000e: calli     <ulong*&,int*&,char*&,char*&,long*&,int*&>
IL_0013: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_get_special_static_data (intptr)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <intptr>
IL_000c: ret       
// (wrapper managed-to-native) System.Globalization.CultureInfo:construct_internal_locale_from_lcid (int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <int>
IL_0019: ret       
// (wrapper managed-to-native) System.Environment:GetMachineConfigPath ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:GetFileAttributes (string,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <string,MonoIOError&>
IL_000d: ret       
// (wrapper remoting-invoke-with-check) System.IO.StreamReader:.ctor (string)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_001a
IL_0014: call      <(wrapper remoting-invoke) System.IO.StreamReader:.ctor (string)>
IL_0019: ret       
IL_001a: call      <System.IO.StreamReader:.ctor (string)>
IL_001f: ret       
// (wrapper remoting-invoke-with-check) System.IO.StreamReader/NullStreamReader:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.StreamReader/NullStreamReader:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.StreamReader/NullStreamReader:.ctor ()>
IL_001e: ret       
// (wrapper remoting-invoke-with-check) System.IO.TextReader/NullTextReader:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.TextReader/NullTextReader:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.TextReader/NullTextReader:.ctor ()>
IL_001e: ret       
// (wrapper alloc) object:Alloc (intptr)
IL_0000: ldarg.0   
IL_0001: ldc.i4.0  
IL_0002: add       
IL_0003: ldind.i   
IL_0004: ldc.i4.s  28
IL_0006: add       
IL_0007: ldind.u4  
IL_0008: stloc.0   
IL_0009: ldloc.0   
IL_000a: ldc.i4.7  
IL_000b: add       
IL_000c: ldc.i4.3  
IL_000d: shr.un    
IL_000e: ldc.i4.3  
IL_000f: shl       
IL_0010: stloc.1   
IL_0011: mono_tls  -8
IL_0017: ldc.i4.s  64
IL_0019: add       
IL_001a: ldloc.1   
IL_001b: add       
IL_001c: stloc.2   
IL_001d: ldloc.2   
IL_001e: ldind.i   
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: ldc.i4    4096
IL_0026: blt.un.s  IL_0037
IL_0028: ldloc.2   
IL_0029: ldloc.3   
IL_002a: ldind.i   
IL_002b: stind.i   
IL_002c: ldloc.3   
IL_002d: ldarg.0   
IL_002e: stind.i   
IL_002f: ldloc.3   
IL_0030: ldc.i4.s  8
IL_0032: add       
IL_0033: ldc.i4.0  
IL_0034: stind.i   
IL_0035: ldloc.3   
IL_0036: ret       
IL_0037: ldarg.0   
IL_0038: mono_icall <mono_object_new_specific>
IL_003e: ret       
// (wrapper remoting-invoke-with-check) System.IO.FileStream:.ctor (string,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.0   
IL_0007: mono_objaddr
IL_0009: ldind.i   
IL_000a: ldc.i4.0  
IL_000b: add       
IL_000c: ldind.i   
IL_000d: mono_classconst <TransparentProxy>
IL_0013: bne.un    IL_001e
IL_0018: call      <(wrapper remoting-invoke) System.IO.FileStream:.ctor (string,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)>
IL_001d: ret       
IL_001e: call      <System.IO.FileStream:.ctor (string,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)>
IL_0023: ret       
// (wrapper remoting-invoke-with-check) System.IO.NullStream:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.NullStream:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.NullStream:.ctor ()>
IL_001e: ret       
// (wrapper managed-to-native) string:InternalSplit (char[],int,int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: mono_ldptr3
IL_0016: calli     <char[],int,int>
IL_001b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:Open (string,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare,System.IO.FileOptions,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.s   5
IL_0008: mono_ldptr2
IL_000e: calli     <string,FileMode,FileAccess,FileShare,FileOptions,MonoIOError&>
IL_0013: ret       
// (wrapper managed-to-native) System.IO.MonoIO:GetFileType (intptr,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MonoIOError&>
IL_000d: ret       
// (wrapper managed-to-native) System.IO.MonoIO:GetLength (intptr,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MonoIOError&>
IL_000d: ret       
// (wrapper managed-to-native) System.IO.MonoIO:Read (intptr,byte[],int,int,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: mono_ldptr2
IL_000c: calli     <intptr,byte[],int,int,MonoIOError&>
IL_0011: ret       
// (wrapper managed-to-native) System.Buffer:BlockCopyInternal (System.Array,int,System.Array,int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: mono_ldptr2
IL_000c: calli     <Array,int,Array,int,int>
IL_0011: ret       
// (wrapper managed-to-managed) string:.ctor (char[],int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: call      <string:CreateString (char[],int,int)>
IL_0009: ret       
// (wrapper managed-to-native) System.Array:GetLowerBound (int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <int>
IL_0019: ret       
// (wrapper managed-to-native) System.Array:ClearInternal (System.Array,int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <Array,int,int>
IL_000e: ret       
// (wrapper managed-to-native) System.Type:EqualsInternal (System.Type)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <Type>
IL_0019: ret       
// (wrapper managed-to-native) System.MonoType:get_IsGenericParameter ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Type:get_IsGenericType ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Type:IsArrayImpl (System.Type)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Type>
IL_000c: ret       
// (wrapper managed-to-native) System.MonoType:IsByRefImpl ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:IsPointerImpl ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Array:CreateInstanceImpl (System.Type,int[],int[])
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <Type,int[],int[]>
IL_000e: ret       
// (wrapper managed-to-native) System.IO.MonoIO:Close (intptr,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MonoIOError&>
IL_000d: ret       
// (wrapper managed-to-native) System.GC:SuppressFinalize (object)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <object>
IL_000c: ret       
// (wrapper managed-to-native) System.Type:internal_from_name (string,bool,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <string,bool,bool>
IL_000e: ret       
// (wrapper managed-to-native) System.MonoType:GetConstructors_internal (System.Reflection.BindingFlags,System.Type)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <BindingFlags,Type>
IL_001a: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethodInfo:get_parameter_info (intptr,System.Reflection.MemberInfo)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MemberInfo>
IL_000d: ret       
// (wrapper managed-to-native) System.MonoType:get_attributes (System.Type)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Type>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethodInfo:get_method_info (intptr,System.Reflection.MonoMethodInfo&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MonoMethodInfo&>
IL_000d: ret       
// (wrapper managed-to-native) System.Reflection.MonoCMethod:InternalInvoke (object,object[],System.Exception&)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: mono_ldptr3
IL_0016: calli     <object,object[],Exception&>
IL_001b: ret       
// (wrapper managed-to-native) System.Security.Cryptography.RNGCryptoServiceProvider:RngOpen ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper runtime-invoke) object:runtime_invoke_void__this__ (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.3   
IL_001c: calli     <>
IL_0021: ldnull    
IL_0022: stloc.0   
IL_0023: leave     IL_0041
IL_0028: pop       
IL_0029: ldarg.2   
IL_002a: ldc.i4.0  
IL_002b: cgt.un    
IL_002d: endfilter 
IL_002f: stloc.1   
IL_0030: ldarg.2   
IL_0031: ldloc.1   
IL_0032: stind.ref 
IL_0033: ldnull    
IL_0034: stloc.0   
IL_0035: ldloc.1   
IL_0036: mono_icall <runtime_invoke_reset_abort>
IL_003c: leave     IL_0041
IL_0041: ldloc.0   
IL_0042: ret       
// (wrapper managed-to-native) System.Security.Cryptography.RNGCryptoServiceProvider:RngInitialize (byte[])
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <byte[]>
IL_000c: ret       
// (wrapper managed-to-native) System.Security.Cryptography.RNGCryptoServiceProvider:RngGetBytes (intptr,byte[])
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,byte[]>
IL_000d: ret       
// (wrapper managed-to-native) System.DateTime:GetNow ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) object:__icall_wrapper___emul_fconv_to_u4 (double)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <double>
IL_000c: ret       
// (wrapper managed-to-native) System.Type:type_is_assignable_from (System.Type,System.Type)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <Type,Type>
IL_000d: ret       
// (wrapper managed-to-native) System.Type:get_IsGenericTypeDefinition ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:GetGenericArguments ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Type:MakeGenericType (System.Type,System.Type[])
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <Type,Type[]>
IL_000d: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_object_castclass (object,intptr)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <object,intptr>
IL_000d: ret       
// (wrapper managed-to-native) System.Environment:internalGetEnvironmentVariable (string)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <string>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:GetExecutingAssembly ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:GetManifestResourceInternal (string,int&,System.Reflection.Module&)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: mono_ldptr3
IL_0016: calli     <string,int&,Module&>
IL_001b: ret       
// (wrapper managed-to-native) System.Environment:GetOSVersionString ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Environment:get_Platform ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Runtime.InteropServices.Marshal:copy_from_unmanaged (intptr,int,System.Array,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: mono_ldptr2
IL_000a: calli     <intptr,int,Array,int>
IL_000f: ret       
// (wrapper managed-to-managed) string:.ctor (char*,int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: call      <string:CreateString (char*,int,int)>
IL_0009: ret       
// (wrapper managed-to-native) System.Array:GetValueImpl (int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <int>
IL_0019: ret       
// (wrapper managed-to-native) System.Array:GetLength (int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <int>
IL_0019: ret       
// (wrapper managed-to-native) System.Array:SetValueImpl (object,int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <object,int>
IL_001a: ret       
// (wrapper managed-to-native) System.MonoType:GetMethodsByName (string,System.Reflection.BindingFlags,bool,System.Type)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: ldarg.s   4
IL_0012: mono_ldptr3
IL_0018: calli     <string,BindingFlags,bool,Type>
IL_001d: ret       
// (wrapper managed-to-native) System.MonoType:GetPropertiesByName (string,System.Reflection.BindingFlags,bool,System.Type)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: ldarg.s   4
IL_0012: mono_ldptr3
IL_0018: calli     <string,BindingFlags,bool,Type>
IL_001d: ret       
// (wrapper managed-to-native) System.Reflection.MonoPropertyInfo:get_property_info (System.Reflection.MonoProperty,System.Reflection.MonoPropertyInfo&,System.Reflection.PInfo)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <MonoProperty,MonoPropertyInfo&,PInfo>
IL_000e: ret       
// (wrapper managed-to-native) System.Type:type_is_subtype_of (System.Type,System.Type,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <Type,Type,bool>
IL_000e: ret       
// (wrapper managed-to-native) System.Delegate:CreateDelegate_internal (System.Type,object,System.Reflection.MethodInfo,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: mono_ldptr2
IL_000a: calli     <Type,object,MethodInfo,bool>
IL_000f: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethod:MakeGenericMethod_impl (System.Type[])
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <Type[]>
IL_0019: ret       
// (wrapper delegate-invoke) System.Reflection.MonoProperty/Getter`2<System.Reflection.MonoMethod, bool>:invoke_bool__this___MonoMethod (System.Reflection.MonoMethod)
IL_0000: mono_ldptr2
IL_0006: ldind.u4  
IL_0007: brfalse   IL_0014
IL_000c: mono_not_taken
IL_000e: mono_icall <mono_thread_interruption_checkpoint>
IL_0014: ldarg.0   
IL_0015: mono_objaddr
IL_0017: ldc.i4.s  88
IL_0019: add       
IL_001a: ldind.ref 
IL_001b: stloc.1   
IL_001c: ldloc.1   
IL_001d: brfalse   IL_002c
IL_0022: mono_not_taken
IL_0024: ldloc.1   
IL_0025: ldarg.1   
IL_0026: callvirt  <System.Reflection.MonoProperty/Getter`2<System.Reflection.MonoMethod, bool>:Invoke (System.Reflection.MonoMethod)>
IL_002b: pop       
IL_002c: ldarg.0   
IL_002d: mono_objaddr
IL_002f: ldc.i4.s  32
IL_0031: add       
IL_0032: ldind.ref 
IL_0033: stloc.0   
IL_0034: ldloc.0   
IL_0035: brfalse   IL_004c
IL_003a: newobj    <System.NotImplementedException:.ctor ()>
IL_003f: dup       
IL_0040: mono_objaddr
IL_0042: ldc.i4.s  32
IL_0044: add       
IL_0045: ldstr     "洀攀渀琀"
IL_004a: stind.ref 
IL_004b: throw     
IL_004c: ldarg.1   
IL_004d: castclass <MonoMethod>
IL_0052: callvirt  <System.Reflection.MonoMethod:get_IsGenericMethod ()>
IL_0057: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethod:get_IsGenericMethod ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper delegate-invoke) System.Reflection.MonoProperty/Getter`2<System.Type, bool>:invoke_bool__this___Type (System.Type)
IL_0000: mono_ldptr2
IL_0006: ldind.u4  
IL_0007: brfalse   IL_0014
IL_000c: mono_not_taken
IL_000e: mono_icall <mono_thread_interruption_checkpoint>
IL_0014: ldarg.0   
IL_0015: mono_objaddr
IL_0017: ldc.i4.s  88
IL_0019: add       
IL_001a: ldind.ref 
IL_001b: stloc.1   
IL_001c: ldloc.1   
IL_001d: brfalse   IL_002c
IL_0022: mono_not_taken
IL_0024: ldloc.1   
IL_0025: ldarg.1   
IL_0026: callvirt  <System.Reflection.MonoProperty/Getter`2<System.Type, bool>:Invoke (System.Type)>
IL_002b: pop       
IL_002c: ldarg.0   
IL_002d: mono_objaddr
IL_002f: ldc.i4.s  32
IL_0031: add       
IL_0032: ldind.ref 
IL_0033: stloc.0   
IL_0034: ldloc.0   
IL_0035: brfalse   IL_004c
IL_003a: newobj    <System.NotImplementedException:.ctor ()>
IL_003f: dup       
IL_0040: mono_objaddr
IL_0042: ldc.i4.s  32
IL_0044: add       
IL_0045: ldstr     "洀攀渀琀"
IL_004a: stind.ref 
IL_004b: throw     
IL_004c: ldarg.1   
IL_004d: castclass <Type>
IL_0052: callvirt  <System.Type:get_IsGenericType ()>
IL_0057: ret       
// (wrapper delegate-invoke) System.Reflection.MonoProperty/Getter`2<System.MonoType, bool>:invoke_bool__this___MonoType (System.MonoType)
IL_0000: mono_ldptr2
IL_0006: ldind.u4  
IL_0007: brfalse   IL_0014
IL_000c: mono_not_taken
IL_000e: mono_icall <mono_thread_interruption_checkpoint>
IL_0014: ldarg.0   
IL_0015: mono_objaddr
IL_0017: ldc.i4.s  88
IL_0019: add       
IL_001a: ldind.ref 
IL_001b: stloc.1   
IL_001c: ldloc.1   
IL_001d: brfalse   IL_002c
IL_0022: mono_not_taken
IL_0024: ldloc.1   
IL_0025: ldarg.1   
IL_0026: callvirt  <System.Reflection.MonoProperty/Getter`2<System.MonoType, bool>:Invoke (System.MonoType)>
IL_002b: pop       
IL_002c: ldarg.0   
IL_002d: mono_objaddr
IL_002f: ldc.i4.s  32
IL_0031: add       
IL_0032: ldind.ref 
IL_0033: stloc.0   
IL_0034: ldloc.0   
IL_0035: brfalse   IL_004c
IL_003a: newobj    <System.NotImplementedException:.ctor ()>
IL_003f: dup       
IL_0040: mono_objaddr
IL_0042: ldc.i4.s  32
IL_0044: add       
IL_0045: ldstr     "洀攀渀琀"
IL_004a: stind.ref 
IL_004b: throw     
IL_004c: ldarg.1   
IL_004d: castclass <MonoType>
IL_0052: callvirt  <System.MonoType:get_IsGenericParameter ()>
IL_0057: ret       
// (wrapper managed-to-native) System.MonoType:get_DeclaringType ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:get_Namespace ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:get_Name ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethod:get_name (System.Reflection.MethodBase)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <MethodBase>
IL_000c: ret       
// (wrapper delegate-invoke) System.Reflection.MonoProperty/Getter`2<System.Type, bool>:invoke_bool__this___Type (System.Type)
IL_0000: mono_ldptr2
IL_0006: ldind.u4  
IL_0007: brfalse   IL_0014
IL_000c: mono_not_taken
IL_000e: mono_icall <mono_thread_interruption_checkpoint>
IL_0014: ldarg.0   
IL_0015: mono_objaddr
IL_0017: ldc.i4.s  88
IL_0019: add       
IL_001a: ldind.ref 
IL_001b: stloc.1   
IL_001c: ldloc.1   
IL_001d: brfalse   IL_002c
IL_0022: mono_not_taken
IL_0024: ldloc.1   
IL_0025: ldarg.1   
IL_0026: callvirt  <System.Reflection.MonoProperty/Getter`2<System.Type, bool>:Invoke (System.Type)>
IL_002b: pop       
IL_002c: ldarg.0   
IL_002d: mono_objaddr
IL_002f: ldc.i4.s  32
IL_0031: add       
IL_0032: ldind.ref 
IL_0033: stloc.0   
IL_0034: ldloc.0   
IL_0035: brfalse   IL_004c
IL_003a: newobj    <System.NotImplementedException:.ctor ()>
IL_003f: dup       
IL_0040: mono_objaddr
IL_0042: ldc.i4.s  32
IL_0044: add       
IL_0045: ldstr     "洀攀渀琀"
IL_004a: stind.ref 
IL_004b: throw     
IL_004c: ldarg.1   
IL_004d: castclass <Type>
IL_0052: callvirt  <System.Type:get_IsGenericTypeDefinition ()>
IL_0057: ret       
// (wrapper managed-to-native) System.Globalization.CultureInfo:construct_number_format ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethod:InternalInvoke (object,object[],System.Exception&)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: mono_ldptr3
IL_0016: calli     <object,object[],Exception&>
IL_001b: ret       
// (wrapper runtime-invoke) object:runtime_invoke_object__this__ (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.3   
IL_001c: calli     <>
IL_0021: stloc.0   
IL_0022: leave     IL_0040
IL_0027: pop       
IL_0028: ldarg.2   
IL_0029: ldc.i4.0  
IL_002a: cgt.un    
IL_002c: endfilter 
IL_002e: stloc.1   
IL_002f: ldarg.2   
IL_0030: ldloc.1   
IL_0031: stind.ref 
IL_0032: ldnull    
IL_0033: stloc.0   
IL_0034: ldloc.1   
IL_0035: mono_icall <runtime_invoke_reset_abort>
IL_003b: leave     IL_0040
IL_0040: ldloc.0   
IL_0041: ret       
// (wrapper managed-to-native) System.Type:GetGenericTypeDefinition_impl ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper delegate-invoke) System.Reflection.MonoProperty/Getter`2<System.Reflection.MemberInfo, int>:invoke_int__this___MemberInfo (System.Reflection.MemberInfo)
IL_0000: mono_ldptr2
IL_0006: ldind.u4  
IL_0007: brfalse   IL_0014
IL_000c: mono_not_taken
IL_000e: mono_icall <mono_thread_interruption_checkpoint>
IL_0014: ldarg.0   
IL_0015: mono_objaddr
IL_0017: ldc.i4.s  88
IL_0019: add       
IL_001a: ldind.ref 
IL_001b: stloc.1   
IL_001c: ldloc.1   
IL_001d: brfalse   IL_002c
IL_0022: mono_not_taken
IL_0024: ldloc.1   
IL_0025: ldarg.1   
IL_0026: callvirt  <System.Reflection.MonoProperty/Getter`2<System.Reflection.MemberInfo, int>:Invoke (System.Reflection.MemberInfo)>
IL_002b: pop       
IL_002c: ldarg.0   
IL_002d: mono_objaddr
IL_002f: ldc.i4.s  32
IL_0031: add       
IL_0032: ldind.ref 
IL_0033: stloc.0   
IL_0034: ldloc.0   
IL_0035: brfalse   IL_004c
IL_003a: newobj    <System.NotImplementedException:.ctor ()>
IL_003f: dup       
IL_0040: mono_objaddr
IL_0042: ldc.i4.s  32
IL_0044: add       
IL_0045: ldstr     "洀攀渀琀"
IL_004a: stind.ref 
IL_004b: throw     
IL_004c: ldarg.1   
IL_004d: castclass <MemberInfo>
IL_0052: callvirt  <System.Reflection.MemberInfo:get_MetadataToken ()>
IL_0057: ret       
// (wrapper managed-to-native) System.Reflection.MemberInfo:get_MetadataToken ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper delegate-invoke) System.Reflection.MonoProperty/Getter`2<System.Type, int>:invoke_int__this___Type (System.Type)
IL_0000: mono_ldptr2
IL_0006: ldind.u4  
IL_0007: brfalse   IL_0014
IL_000c: mono_not_taken
IL_000e: mono_icall <mono_thread_interruption_checkpoint>
IL_0014: ldarg.0   
IL_0015: mono_objaddr
IL_0017: ldc.i4.s  88
IL_0019: add       
IL_001a: ldind.ref 
IL_001b: stloc.1   
IL_001c: ldloc.1   
IL_001d: brfalse   IL_002c
IL_0022: mono_not_taken
IL_0024: ldloc.1   
IL_0025: ldarg.1   
IL_0026: callvirt  <System.Reflection.MonoProperty/Getter`2<System.Type, int>:Invoke (System.Type)>
IL_002b: pop       
IL_002c: ldarg.0   
IL_002d: mono_objaddr
IL_002f: ldc.i4.s  32
IL_0031: add       
IL_0032: ldind.ref 
IL_0033: stloc.0   
IL_0034: ldloc.0   
IL_0035: brfalse   IL_004c
IL_003a: newobj    <System.NotImplementedException:.ctor ()>
IL_003f: dup       
IL_0040: mono_objaddr
IL_0042: ldc.i4.s  32
IL_0044: add       
IL_0045: ldstr     "洀攀渀琀"
IL_004a: stind.ref 
IL_004b: throw     
IL_004c: ldarg.1   
IL_004d: castclass <Type>
IL_0052: callvirt  <System.Type:get_GenericParameterPosition ()>
IL_0057: ret       
// (wrapper managed-to-native) System.Type:GetGenericParameterPosition ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper delegate-invoke) System.Reflection.MonoProperty/Getter`2<System.MonoType, System.Reflection.MethodBase>:invoke_MethodBase__this___MonoType (System.MonoType)
IL_0000: mono_ldptr2
IL_0006: ldind.u4  
IL_0007: brfalse   IL_0014
IL_000c: mono_not_taken
IL_000e: mono_icall <mono_thread_interruption_checkpoint>
IL_0014: ldarg.0   
IL_0015: mono_objaddr
IL_0017: ldc.i4.s  88
IL_0019: add       
IL_001a: ldind.ref 
IL_001b: stloc.1   
IL_001c: ldloc.1   
IL_001d: brfalse   IL_002c
IL_0022: mono_not_taken
IL_0024: ldloc.1   
IL_0025: ldarg.1   
IL_0026: callvirt  <System.Reflection.MonoProperty/Getter`2<System.MonoType, System.Reflection.MethodBase>:Invoke (System.MonoType)>
IL_002b: pop       
IL_002c: ldarg.0   
IL_002d: mono_objaddr
IL_002f: ldc.i4.s  32
IL_0031: add       
IL_0032: ldind.ref 
IL_0033: stloc.0   
IL_0034: ldloc.0   
IL_0035: brfalse   IL_004c
IL_003a: newobj    <System.NotImplementedException:.ctor ()>
IL_003f: dup       
IL_0040: mono_objaddr
IL_0042: ldc.i4.s  32
IL_0044: add       
IL_0045: ldstr     "洀攀渀琀"
IL_004a: stind.ref 
IL_004b: throw     
IL_004c: ldarg.1   
IL_004d: castclass <MonoType>
IL_0052: callvirt  <System.MonoType:get_DeclaringMethod ()>
IL_0057: ret       
// (wrapper managed-to-native) System.MonoType:get_DeclaringMethod ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:GetArrayRank ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:GetElementType ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:GetField (string,System.Reflection.BindingFlags)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <string,BindingFlags>
IL_001a: ret       
// (wrapper managed-to-native) System.Reflection.MonoField:GetParentType (bool)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <bool>
IL_0019: ret       
// (wrapper managed-to-native) System.IO.MonoIO:GetFileStat (string,System.IO.MonoIOStat&,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <string,MonoIOStat&,MonoIOError&>
IL_000e: ret       
// (wrapper managed-to-native) System.CurrentSystemTimeZone:GetTimeZoneData (int,long[]&,string[]&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <int,long[]&,string[]&>
IL_000e: ret       
// (wrapper managed-to-native) System.IO.MonoIO:GetFileSystemEntries (string,string,int,int,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: mono_ldptr2
IL_000c: calli     <string,string,int,int,MonoIOError&>
IL_0011: ret       
// (wrapper managed-to-managed) string[]:System.Collections.Generic.IEnumerable`1.GetEnumerator ()
IL_0000: ldarg.0   
IL_0001: call      <System.Array:InternalArray__IEnumerable_GetEnumerator<string> ()>
IL_0006: ret       
// (wrapper managed-to-native) System.Array:GetGenericValueImpl (int,string&)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <int,string&>
IL_001a: ret       
// (wrapper managed-to-native) System.Text.Encoding:InternalCodePage (int&)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <int&>
IL_000c: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_ConsoleError ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper remoting-invoke-with-check) System.IO.FileStream:.ctor (intptr,System.IO.FileAccess,bool,int,bool,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.s   5
IL_0008: ldarg.s   6
IL_000a: ldarg.0   
IL_000b: mono_objaddr
IL_000d: ldind.i   
IL_000e: ldc.i4.0  
IL_000f: add       
IL_0010: ldind.i   
IL_0011: mono_classconst <TransparentProxy>
IL_0017: bne.un    IL_0022
IL_001c: call      <(wrapper remoting-invoke) System.IO.FileStream:.ctor (intptr,System.IO.FileAccess,bool,int,bool,bool)>
IL_0021: ret       
IL_0022: call      <System.IO.FileStream:.ctor (intptr,System.IO.FileAccess,bool,int,bool,bool)>
IL_0027: ret       
// (wrapper remoting-invoke-with-check) System.IO.UnexceptionalStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_001b
IL_0015: call      <(wrapper remoting-invoke) System.IO.UnexceptionalStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_001a: ret       
IL_001b: call      <System.IO.UnexceptionalStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_0020: ret       
// (wrapper remoting-invoke-with-check) System.IO.StreamWriter:.ctor (System.IO.Stream,System.Text.Encoding,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.0   
IL_0005: mono_objaddr
IL_0007: ldind.i   
IL_0008: ldc.i4.0  
IL_0009: add       
IL_000a: ldind.i   
IL_000b: mono_classconst <TransparentProxy>
IL_0011: bne.un    IL_001c
IL_0016: call      <(wrapper remoting-invoke) System.IO.StreamWriter:.ctor (System.IO.Stream,System.Text.Encoding,int)>
IL_001b: ret       
IL_001c: call      <System.IO.StreamWriter:.ctor (System.IO.Stream,System.Text.Encoding,int)>
IL_0021: ret       
// (wrapper remoting-invoke-with-check) System.IO.TextWriter/NullTextWriter:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.TextWriter/NullTextWriter:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.TextWriter/NullTextWriter:.ctor ()>
IL_001e: ret       
// (wrapper managed-to-native) System.Environment:get_NewLine ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper remoting-invoke-with-check) System.IO.SynchronizedWriter:.ctor (System.IO.TextWriter,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_001b
IL_0015: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:.ctor (System.IO.TextWriter,bool)>
IL_001a: ret       
IL_001b: call      <System.IO.SynchronizedWriter:.ctor (System.IO.TextWriter,bool)>
IL_0020: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_ConsoleOutput ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.ConsoleDriver:Isatty (intptr)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <intptr>
IL_000c: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_ConsoleInput ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper remoting-invoke-with-check) System.IO.CStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_001b
IL_0015: call      <(wrapper remoting-invoke) System.IO.CStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_001a: ret       
IL_001b: call      <System.IO.CStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_0020: ret       
// (wrapper remoting-invoke-with-check) System.IO.CStreamReader:.ctor (System.IO.Stream,System.Text.Encoding)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_001b
IL_0015: call      <(wrapper remoting-invoke) System.IO.CStreamReader:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_001a: ret       
IL_001b: call      <System.IO.CStreamReader:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_0020: ret       
// (wrapper managed-to-native) System.IO.MonoIO:Write (intptr,byte[],int,int,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: mono_ldptr2
IL_000c: calli     <intptr,byte[],int,int,MonoIOError&>
IL_0011: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_object_isinst (object,intptr)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <object,intptr>
IL_000d: ret       
// (wrapper managed-to-managed) string[]:System.Collections.Generic.ICollection`1.Contains (string)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: call      <System.Array:InternalArray__ICollection_Contains<string> (string)>
IL_0007: ret       
// (wrapper managed-to-native) System.IO.MonoIO:GetCurrentDirectory (System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <MonoIOError&>
IL_000c: ret       
// (wrapper remoting-invoke-with-check) System.IO.StreamReader:.ctor (System.IO.Stream)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_001a
IL_0014: call      <(wrapper remoting-invoke) System.IO.StreamReader:.ctor (System.IO.Stream)>
IL_0019: ret       
IL_001a: call      <System.IO.StreamReader:.ctor (System.IO.Stream)>
IL_001f: ret       
// (wrapper managed-to-native) System.MonoType:get_Module ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper remoting-invoke-with-check) System.IO.FileInfo:.ctor (string)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_001a
IL_0014: call      <(wrapper remoting-invoke) System.IO.FileInfo:.ctor (string)>
IL_0019: ret       
IL_001a: call      <System.IO.FileInfo:.ctor (string)>
IL_001f: ret       
// (wrapper managed-to-native) System.IO.MonoIO:Seek (intptr,long,System.IO.SeekOrigin,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: mono_ldptr2
IL_000a: calli     <intptr,long,SeekOrigin,MonoIOError&>
IL_000f: ret       
// (wrapper managed-to-managed) string:.ctor (char[])
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: call      <string:CreateString (char[])>
IL_0007: ret       
// (wrapper remoting-invoke-with-check) System.IO.MemoryStream:.ctor (byte[])
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_001a
IL_0014: call      <(wrapper remoting-invoke) System.IO.MemoryStream:.ctor (byte[])>
IL_0019: ret       
IL_001a: call      <System.IO.MemoryStream:.ctor (byte[])>
IL_001f: ret       
// (wrapper remoting-invoke-with-check) System.IO.StringReader:.ctor (string)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_001a
IL_0014: call      <(wrapper remoting-invoke) System.IO.StringReader:.ctor (string)>
IL_0019: ret       
IL_001a: call      <System.IO.StringReader:.ctor (string)>
IL_001f: ret       
// (wrapper runtime-invoke) object:runtime_invoke_void__this___int (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.1   
IL_001c: ldind.i   
IL_001d: ldind.i4  
IL_001e: ldarg.3   
IL_001f: calli     <int>
IL_0024: ldnull    
IL_0025: stloc.0   
IL_0026: leave     IL_0044
IL_002b: pop       
IL_002c: ldarg.2   
IL_002d: ldc.i4.0  
IL_002e: cgt.un    
IL_0030: endfilter 
IL_0032: stloc.1   
IL_0033: ldarg.2   
IL_0034: ldloc.1   
IL_0035: stind.ref 
IL_0036: ldnull    
IL_0037: stloc.0   
IL_0038: ldloc.1   
IL_0039: mono_icall <runtime_invoke_reset_abort>
IL_003f: leave     IL_0044
IL_0044: ldloc.0   
IL_0045: ret       
// (wrapper managed-to-native) System.MonoEnumInfo:get_enum_info (System.Type,System.MonoEnumInfo&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <Type,MonoEnumInfo&>
IL_000d: ret       
// (wrapper managed-to-native) System.MonoType:getFullName (bool,bool)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <bool,bool>
IL_001a: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:get_fullname ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Enum:get_value ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Enum:get_underlying_type (System.Type)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Type>
IL_000c: ret       
// (wrapper managed-to-native) System.MonoCustomAttrs:IsDefinedInternal (System.Reflection.ICustomAttributeProvider,System.Type)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <ICustomAttributeProvider,Type>
IL_000d: ret       
// (wrapper managed-to-native) System.Type:GetTypeCodeInternal (System.Type)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Type>
IL_000c: ret       
// (wrapper managed-to-native) System.Enum:get_hashcode ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Enum:ToObject (System.Type,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <Type,object>
IL_000d: ret       
// (wrapper managed-to-native) System.Globalization.CompareInfo:internal_index (string,int,int,string,System.Globalization.CompareOptions,bool)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: ldarg.s   4
IL_0012: ldarg.s   5
IL_0014: ldarg.s   6
IL_0016: mono_ldptr3
IL_001c: calli     <string,int,int,string,CompareOptions,bool>
IL_0021: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_ldftn (intptr)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <intptr>
IL_000c: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_delegate_ctor (object,object,intptr)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <object,object,intptr>
IL_000e: ret       
// (wrapper remoting-invoke-with-check) System.IO.MemoryStream:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.MemoryStream:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.MemoryStream:.ctor ()>
IL_001e: ret       
// (wrapper managed-to-native) System.AppDomain:getCurDomain ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:get_BaseDirectory ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.AppDomain:get_BaseDirectory ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.AppDomain:get_BaseDirectory ()>
IL_0031: ret       
IL_0032: call      <System.AppDomain:get_BaseDirectory ()>
IL_0037: ret       
// (wrapper managed-to-native) System.AppDomain:getSetup ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.AppDomain:getRootDomain ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:get_SetupInformation ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.AppDomain:get_SetupInformation ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.AppDomain:get_SetupInformation ()>
IL_0031: ret       
IL_0032: call      <System.AppDomain:get_SetupInformation ()>
IL_0037: ret       
// (wrapper managed-to-native) System.AppDomain:createDomain (string,System.AppDomainSetup)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <string,AppDomainSetup>
IL_000d: ret       
// (wrapper runtime-invoke) object:runtime_invoke_void__this___object (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.1   
IL_001c: ldind.ref 
IL_001d: ldarg.3   
IL_001e: calli     <object>
IL_0023: ldnull    
IL_0024: stloc.0   
IL_0025: leave     IL_0043
IL_002a: pop       
IL_002b: ldarg.2   
IL_002c: ldc.i4.0  
IL_002d: cgt.un    
IL_002f: endfilter 
IL_0031: stloc.1   
IL_0032: ldarg.2   
IL_0033: ldloc.1   
IL_0034: stind.ref 
IL_0035: ldnull    
IL_0036: stloc.0   
IL_0037: ldloc.1   
IL_0038: mono_icall <runtime_invoke_reset_abort>
IL_003e: leave     IL_0043
IL_0043: ldloc.0   
IL_0044: ret       
// (wrapper managed-to-native) System.AppDomain:InternalGetDefaultContext ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.AppDomain:InternalGetContext ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.AppDomain:InternalPushDomainRef (System.AppDomain)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <AppDomain>
IL_000c: ret       
// (wrapper managed-to-native) System.AppDomain:InternalSetDomain (System.AppDomain)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <AppDomain>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethod:InternalInvoke (object,object[],System.Exception&)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: mono_ldptr3
IL_0016: calli     <object,object[],Exception&>
IL_001b: ret       
// (wrapper runtime-invoke) object:runtime_invoke_object__this__ (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.3   
IL_001c: calli     <>
IL_0021: stloc.0   
IL_0022: leave     IL_0040
IL_0027: pop       
IL_0028: ldarg.2   
IL_0029: ldc.i4.0  
IL_002a: cgt.un    
IL_002c: endfilter 
IL_002e: stloc.1   
IL_002f: ldarg.2   
IL_0030: ldloc.1   
IL_0031: stind.ref 
IL_0032: ldnull    
IL_0033: stloc.0   
IL_0034: ldloc.1   
IL_0035: mono_icall <runtime_invoke_reset_abort>
IL_003b: leave     IL_0040
IL_0040: ldloc.0   
IL_0041: ret       
// (wrapper managed-to-native) System.AppDomain:getCurDomain ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper runtime-invoke) object:runtime_invoke_void (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.3   
IL_001b: calli     <>
IL_0020: ldnull    
IL_0021: stloc.0   
IL_0022: leave     IL_0040
IL_0027: pop       
IL_0028: ldarg.2   
IL_0029: ldc.i4.0  
IL_002a: cgt.un    
IL_002c: endfilter 
IL_002e: stloc.1   
IL_002f: ldarg.2   
IL_0030: ldloc.1   
IL_0031: stind.ref 
IL_0032: ldnull    
IL_0033: stloc.0   
IL_0034: ldloc.1   
IL_0035: mono_icall <runtime_invoke_reset_abort>
IL_003b: leave     IL_0040
IL_0040: ldloc.0   
IL_0041: ret       
// (wrapper alloc) object:Alloc (intptr)
IL_0000: ldarg.0   
IL_0001: ldc.i4.0  
IL_0002: add       
IL_0003: ldind.i   
IL_0004: ldc.i4.s  28
IL_0006: add       
IL_0007: ldind.u4  
IL_0008: stloc.0   
IL_0009: ldloc.0   
IL_000a: ldc.i4.7  
IL_000b: add       
IL_000c: ldc.i4.3  
IL_000d: shr.un    
IL_000e: ldc.i4.3  
IL_000f: shl       
IL_0010: stloc.1   
IL_0011: mono_tls  -8
IL_0017: ldc.i4.s  64
IL_0019: add       
IL_001a: ldloc.1   
IL_001b: add       
IL_001c: stloc.2   
IL_001d: ldloc.2   
IL_001e: ldind.i   
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: ldc.i4    4096
IL_0026: blt.un.s  IL_004d
IL_0028: ldloc.2   
IL_0029: ldloc.3   
IL_002a: ldind.i   
IL_002b: stind.i   
IL_002c: ldloc.3   
IL_002d: ldarg.0   
IL_002e: stind.i   
IL_002f: ldloc.3   
IL_0030: ldloc.0   
IL_0031: add       
IL_0032: stloc.s   5
IL_0034: ldloc.3   
IL_0035: ldc.i4.s  8
IL_0037: add       
IL_0038: stloc.s   4
IL_003a: ldloc.s   4
IL_003c: ldc.i4.0  
IL_003d: stind.i   
IL_003e: ldloc.s   4
IL_0040: ldc.i4.s  8
IL_0042: add       
IL_0043: stloc.s   4
IL_0045: ldloc.s   4
IL_0047: ldloc.s   5
IL_0049: blt.un.s  IL_003a
IL_004b: ldloc.3   
IL_004c: ret       
IL_004d: ldarg.0   
IL_004e: mono_icall <mono_object_new_specific>
IL_0054: ret       
// (wrapper stelemref) object:stelemref (object,intptr,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldelema   <Object>
IL_0007: stloc.2   
IL_0008: ldarg.2   
IL_0009: brfalse   IL_004f
IL_000e: ldarg.0   
IL_000f: mono_objaddr
IL_0011: ldind.i   
IL_0012: mono_objaddr
IL_0014: ldind.i   
IL_0015: mono_objaddr
IL_0017: ldind.i   
IL_0018: stloc.0   
IL_0019: ldarg.2   
IL_001a: mono_objaddr
IL_001c: ldind.i   
IL_001d: mono_objaddr
IL_001f: ldind.i   
IL_0020: stloc.1   
IL_0021: ldloc.1   
IL_0022: mono_objaddr
IL_0024: ldc.i4.s  24
IL_0026: add       
IL_0027: ldind.u2  
IL_0028: ldloc.0   
IL_0029: mono_objaddr
IL_002b: ldc.i4.s  24
IL_002d: add       
IL_002e: ldind.u2  
IL_002f: blt.un    IL_0053
IL_0034: ldloc.1   
IL_0035: mono_objaddr
IL_0037: ldc.i4.s  16
IL_0039: add       
IL_003a: ldind.i   
IL_003b: ldloc.0   
IL_003c: mono_objaddr
IL_003e: ldc.i4.s  24
IL_0040: add       
IL_0041: ldind.u2  
IL_0042: ldc.i4.1  
IL_0043: sub       
IL_0044: ldc.i4.s  8
IL_0046: mul       
IL_0047: add       
IL_0048: ldind.i   
IL_0049: ldloc.0   
IL_004a: bne.un    IL_0053
IL_004f: ldloc.2   
IL_0050: ldarg.2   
IL_0051: stind.ref 
IL_0052: ret       
IL_0053: ldarg.2   
IL_0054: ldloc.0   
IL_0055: mono_icall <mono_object_isinst>
IL_005b: brtrue    IL_004f
IL_0060: newobj    <System.ArrayTypeMismatchException:.ctor ()>
IL_0065: throw     
IL_0066: ret       
// (wrapper managed-to-native) System.Array:FastCopy (System.Array,int,System.Array,int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: mono_ldptr2
IL_000c: calli     <Array,int,Array,int,int>
IL_0011: ret       
// (wrapper managed-to-native) System.MonoType:getFullName (bool,bool)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <bool,bool>
IL_001a: ret       
// (wrapper managed-to-native) System.Array:GetValueImpl (int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <int>
IL_0019: ret       
// (wrapper managed-to-native) object:MemberwiseClone ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Threading.Thread:GetCachedCurrentCulture ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Threading.Thread:GetSerializedCurrentCulture ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Globalization.CultureInfo:construct_internal_locale_from_current_locale (System.Globalization.CultureInfo)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <CultureInfo>
IL_000c: ret       
// (wrapper managed-to-native) System.Threading.Thread:SetCachedCurrentCulture (System.Globalization.CultureInfo)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <CultureInfo>
IL_0019: ret       
// (wrapper managed-to-native) System.NumberFormatter:GetFormatterTables (ulong*&,int*&,char*&,char*&,long*&,int*&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.s   5
IL_0008: mono_ldptr2
IL_000e: calli     <ulong*&,int*&,char*&,char*&,long*&,int*&>
IL_0013: ret       
// (wrapper managed-to-native) System.Globalization.CultureInfo:construct_internal_locale_from_lcid (int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <int>
IL_0019: ret       
// (wrapper alloc) object:Alloc (intptr,int)
IL_0000: ldarg.1   
IL_0001: ldc.i4.1  
IL_0002: add       
IL_0003: ldc.i4.1  
IL_0004: shl       
IL_0005: ldc.i4.s  20
IL_0007: add       
IL_0008: stloc.0   
IL_0009: ldloc.0   
IL_000a: ldc.i4    512
IL_000f: bgt.un.s  IL_0051
IL_0011: ldloc.0   
IL_0012: ldc.i4.s  24
IL_0014: ble.un.s  IL_0051
IL_0016: ldloc.0   
IL_0017: ldc.i4.7  
IL_0018: add       
IL_0019: ldc.i4.3  
IL_001a: shr.un    
IL_001b: ldc.i4.3  
IL_001c: shl       
IL_001d: stloc.1   
IL_001e: mono_tls  -8
IL_0024: ldc.i4.s  64
IL_0026: add       
IL_0027: ldloc.1   
IL_0028: add       
IL_0029: stloc.2   
IL_002a: ldloc.2   
IL_002b: ldind.i   
IL_002c: stloc.3   
IL_002d: ldloc.3   
IL_002e: ldc.i4    4096
IL_0033: blt.un.s  IL_0051
IL_0035: ldloc.2   
IL_0036: ldloc.3   
IL_0037: ldind.i   
IL_0038: stind.i   
IL_0039: ldloc.3   
IL_003a: ldarg.0   
IL_003b: stind.i   
IL_003c: ldloc.3   
IL_003d: ldc.i4.s  8
IL_003f: add       
IL_0040: ldc.i4.0  
IL_0041: stind.i   
IL_0042: ldloc.3   
IL_0043: ldc.i4.s  16
IL_0045: add       
IL_0046: ldarg.1   
IL_0047: stind.i4  
IL_0048: ldloc.3   
IL_0049: ldloc.0   
IL_004a: ldc.i4.2  
IL_004b: sub       
IL_004c: add       
IL_004d: ldc.i4.0  
IL_004e: stind.i2  
IL_004f: ldloc.3   
IL_0050: ret       
IL_0051: ldarg.1   
IL_0052: mono_icall <mono_string_alloc>
IL_0058: ret       
// (wrapper managed-to-native) System.Environment:GetMachineConfigPath ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_VolumeSeparatorChar ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_DirectorySeparatorChar ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_AltDirectorySeparatorChar ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_PathSeparator ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) char:GetDataTablePointers (byte*&,byte*&,double*&,uint16*&,uint16*&,uint16*&,uint16*&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.s   5
IL_0008: ldarg.s   6
IL_000a: mono_ldptr2
IL_0010: calli     <byte*&,byte*&,double*&,uint16*&,uint16*&,uint16*&,uint16*&>
IL_0015: ret       
// (wrapper managed-to-managed) string:.ctor (char,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: call      <string:CreateString (char,int)>
IL_0008: ret       
// (wrapper managed-to-native) System.IO.MonoIO:GetFileAttributes (string,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <string,MonoIOError&>
IL_000d: ret       
// (wrapper remoting-invoke-with-check) System.IO.StreamReader:.ctor (string)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_001a
IL_0014: call      <(wrapper remoting-invoke) System.IO.StreamReader:.ctor (string)>
IL_0019: ret       
IL_001a: call      <System.IO.StreamReader:.ctor (string)>
IL_001f: ret       
// (wrapper remoting-invoke-with-check) System.IO.StreamReader/NullStreamReader:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.StreamReader/NullStreamReader:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.StreamReader/NullStreamReader:.ctor ()>
IL_001e: ret       
// (wrapper remoting-invoke-with-check) System.IO.TextReader/NullTextReader:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.TextReader/NullTextReader:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.TextReader/NullTextReader:.ctor ()>
IL_001e: ret       
// (wrapper alloc) object:Alloc (intptr)
IL_0000: ldarg.0   
IL_0001: ldc.i4.0  
IL_0002: add       
IL_0003: ldind.i   
IL_0004: ldc.i4.s  28
IL_0006: add       
IL_0007: ldind.u4  
IL_0008: stloc.0   
IL_0009: ldloc.0   
IL_000a: ldc.i4.7  
IL_000b: add       
IL_000c: ldc.i4.3  
IL_000d: shr.un    
IL_000e: ldc.i4.3  
IL_000f: shl       
IL_0010: stloc.1   
IL_0011: mono_tls  -8
IL_0017: ldc.i4.s  64
IL_0019: add       
IL_001a: ldloc.1   
IL_001b: add       
IL_001c: stloc.2   
IL_001d: ldloc.2   
IL_001e: ldind.i   
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: ldc.i4    4096
IL_0026: blt.un.s  IL_0037
IL_0028: ldloc.2   
IL_0029: ldloc.3   
IL_002a: ldind.i   
IL_002b: stind.i   
IL_002c: ldloc.3   
IL_002d: ldarg.0   
IL_002e: stind.i   
IL_002f: ldloc.3   
IL_0030: ldc.i4.s  8
IL_0032: add       
IL_0033: ldc.i4.0  
IL_0034: stind.i   
IL_0035: ldloc.3   
IL_0036: ret       
IL_0037: ldarg.0   
IL_0038: mono_icall <mono_object_new_specific>
IL_003e: ret       
// (wrapper remoting-invoke-with-check) System.IO.FileStream:.ctor (string,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.0   
IL_0007: mono_objaddr
IL_0009: ldind.i   
IL_000a: ldc.i4.0  
IL_000b: add       
IL_000c: ldind.i   
IL_000d: mono_classconst <TransparentProxy>
IL_0013: bne.un    IL_001e
IL_0018: call      <(wrapper remoting-invoke) System.IO.FileStream:.ctor (string,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)>
IL_001d: ret       
IL_001e: call      <System.IO.FileStream:.ctor (string,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)>
IL_0023: ret       
// (wrapper remoting-invoke-with-check) System.IO.NullStream:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.NullStream:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.NullStream:.ctor ()>
IL_001e: ret       
// (wrapper managed-to-native) string:InternalSplit (char[],int,int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: mono_ldptr3
IL_0016: calli     <char[],int,int>
IL_001b: ret       
// (wrapper managed-to-native) System.Security.SecurityManager:get_SecurityEnabled ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.IO.MonoIO:Open (string,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare,System.IO.FileOptions,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.s   5
IL_0008: mono_ldptr2
IL_000e: calli     <string,FileMode,FileAccess,FileShare,FileOptions,MonoIOError&>
IL_0013: ret       
// (wrapper managed-to-native) System.IO.MonoIO:GetFileType (intptr,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MonoIOError&>
IL_000d: ret       
// (wrapper managed-to-native) System.IO.MonoIO:GetLength (intptr,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MonoIOError&>
IL_000d: ret       
// (wrapper managed-to-native) System.IO.MonoIO:Read (intptr,byte[],int,int,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: mono_ldptr2
IL_000c: calli     <intptr,byte[],int,int,MonoIOError&>
IL_0011: ret       
// (wrapper managed-to-native) System.Buffer:BlockCopyInternal (System.Array,int,System.Array,int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: mono_ldptr2
IL_000c: calli     <Array,int,Array,int,int>
IL_0011: ret       
// (wrapper managed-to-managed) string:.ctor (char[],int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: call      <string:CreateString (char[],int,int)>
IL_0009: ret       
// (wrapper managed-to-native) System.Array:GetLowerBound (int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <int>
IL_0019: ret       
// (wrapper managed-to-native) System.Array:ClearInternal (System.Array,int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <Array,int,int>
IL_000e: ret       
// (wrapper managed-to-native) System.Type:EqualsInternal (System.Type)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <Type>
IL_0019: ret       
// (wrapper managed-to-native) System.MonoType:get_IsGenericParameter ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Type:get_IsGenericType ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Type:IsArrayImpl (System.Type)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Type>
IL_000c: ret       
// (wrapper managed-to-native) System.MonoType:IsByRefImpl ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:IsPointerImpl ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Array:CreateInstanceImpl (System.Type,int[],int[])
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <Type,int[],int[]>
IL_000e: ret       
// (wrapper managed-to-native) System.IO.MonoIO:Close (intptr,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MonoIOError&>
IL_000d: ret       
// (wrapper managed-to-native) System.GC:SuppressFinalize (object)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <object>
IL_000c: ret       
// (wrapper managed-to-native) System.Type:internal_from_name (string,bool,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <string,bool,bool>
IL_000e: ret       
// (wrapper managed-to-native) System.MonoType:GetConstructors_internal (System.Reflection.BindingFlags,System.Type)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <BindingFlags,Type>
IL_001a: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethodInfo:get_parameter_info (intptr,System.Reflection.MemberInfo)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MemberInfo>
IL_000d: ret       
// (wrapper managed-to-native) System.MonoType:get_attributes (System.Type)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Type>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethodInfo:get_method_info (intptr,System.Reflection.MonoMethodInfo&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,MonoMethodInfo&>
IL_000d: ret       
// (wrapper managed-to-native) System.Reflection.MonoCMethod:InternalInvoke (object,object[],System.Exception&)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: mono_ldptr3
IL_0016: calli     <object,object[],Exception&>
IL_001b: ret       
// (wrapper managed-to-native) System.Security.Cryptography.RNGCryptoServiceProvider:RngOpen ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper runtime-invoke) object:runtime_invoke_void__this__ (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.3   
IL_001c: calli     <>
IL_0021: ldnull    
IL_0022: stloc.0   
IL_0023: leave     IL_0041
IL_0028: pop       
IL_0029: ldarg.2   
IL_002a: ldc.i4.0  
IL_002b: cgt.un    
IL_002d: endfilter 
IL_002f: stloc.1   
IL_0030: ldarg.2   
IL_0031: ldloc.1   
IL_0032: stind.ref 
IL_0033: ldnull    
IL_0034: stloc.0   
IL_0035: ldloc.1   
IL_0036: mono_icall <runtime_invoke_reset_abort>
IL_003c: leave     IL_0041
IL_0041: ldloc.0   
IL_0042: ret       
// (wrapper managed-to-native) System.Security.Cryptography.RNGCryptoServiceProvider:RngInitialize (byte[])
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <byte[]>
IL_000c: ret       
// (wrapper managed-to-native) System.Security.Cryptography.RNGCryptoServiceProvider:RngGetBytes (intptr,byte[])
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,byte[]>
IL_000d: ret       
// (wrapper managed-to-native) System.AppDomain:InternalGetDefaultContext ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.MonoType:GetMethodsByName (string,System.Reflection.BindingFlags,bool,System.Type)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: ldarg.s   4
IL_0012: mono_ldptr3
IL_0018: calli     <string,BindingFlags,bool,Type>
IL_001d: ret       
// (wrapper managed-to-native) System.Runtime.Remoting.RemotingServices:IsTransparentProxy (object)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <object>
IL_000c: ret       
// (wrapper remoting-invoke-with-check) System.MarshalByRefObject:get_ObjectIdentity ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.MarshalByRefObject:get_ObjectIdentity ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.MarshalByRefObject:get_ObjectIdentity ()>
IL_0031: ret       
IL_0032: call      <System.MarshalByRefObject:get_ObjectIdentity ()>
IL_0037: ret       
// (wrapper managed-to-native) System.Environment:get_TickCount ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Globalization.CultureInfo:construct_number_format ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Type:type_is_assignable_from (System.Type,System.Type)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <Type,Type>
IL_000d: ret       
// (wrapper remoting-invoke-with-check) System.MarshalByRefObject:set_ObjectIdentity (System.Runtime.Remoting.ServerIdentity)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_0033
IL_0014: ldarg.0   
IL_0015: mono_objaddr
IL_0017: ldc.i4.s  16
IL_0019: add       
IL_001a: ldind.ref 
IL_001b: mono_objaddr
IL_001d: ldc.i4.s  40
IL_001f: add       
IL_0020: ldind.i4  
IL_0021: ldc.i4.m1 
IL_0022: beq       IL_002d
IL_0027: call      <(wrapper xdomain-invoke) System.MarshalByRefObject:set_ObjectIdentity (System.Runtime.Remoting.ServerIdentity)>
IL_002c: ret       
IL_002d: call      <(wrapper remoting-invoke) System.MarshalByRefObject:set_ObjectIdentity (System.Runtime.Remoting.ServerIdentity)>
IL_0032: ret       
IL_0033: call      <System.MarshalByRefObject:set_ObjectIdentity (System.Runtime.Remoting.ServerIdentity)>
IL_0038: ret       
// (wrapper managed-to-native) System.Threading.Thread:GetDomainID ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.AppDomain:InternalGetProcessGuid (string)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <string>
IL_000c: ret       
// (wrapper managed-to-native) System.MonoType:get_BaseType ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:GetInterfaces ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper remoting-invoke-with-check) System.IO.MemoryStream:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.MemoryStream:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.MemoryStream:.ctor ()>
IL_001e: ret       
// (wrapper managed-to-native) System.MonoType:get_Assembly ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:get_fullname ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Environment:internalGetEnvironmentVariable (string)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <string>
IL_000c: ret       
// (wrapper managed-to-native) System.MonoType:IsPrimitiveImpl ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Type:GetTypeCodeInternal (System.Type)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Type>
IL_000c: ret       
// (wrapper managed-to-native) System.Type:type_is_subtype_of (System.Type,System.Type,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <Type,Type,bool>
IL_000e: ret       
// (wrapper managed-to-native) System.Array:Clone ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoCustomAttrs:IsDefinedInternal (System.Reflection.ICustomAttributeProvider,System.Type)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <ICustomAttributeProvider,Type>
IL_000d: ret       
// (wrapper managed-to-native) System.Enum:get_hashcode ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:DefineInternalDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_0034
IL_0015: ldarg.0   
IL_0016: mono_objaddr
IL_0018: ldc.i4.s  16
IL_001a: add       
IL_001b: ldind.ref 
IL_001c: mono_objaddr
IL_001e: ldc.i4.s  40
IL_0020: add       
IL_0021: ldind.i4  
IL_0022: ldc.i4.m1 
IL_0023: beq       IL_002e
IL_0028: call      <(wrapper xdomain-invoke) System.AppDomain:DefineInternalDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)>
IL_002d: ret       
IL_002e: call      <(wrapper remoting-invoke) System.AppDomain:DefineInternalDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)>
IL_0033: ret       
IL_0034: call      <System.AppDomain:DefineInternalDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)>
IL_0039: ret       
// (wrapper managed-to-native) System.MonoEnumInfo:get_enum_info (System.Type,System.MonoEnumInfo&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <Type,MonoEnumInfo&>
IL_000d: ret       
// (wrapper managed-to-native) System.Array:GetLength (int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <int>
IL_0019: ret       
// (wrapper managed-to-native) System.Array:SetValueImpl (object,int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <object,int>
IL_001a: ret       
// (wrapper managed-to-native) System.Enum:ToObject (System.Type,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <Type,object>
IL_000d: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:GetExecutingAssembly ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:GetManifestResourceInternal (string,int&,System.Reflection.Module&)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: mono_ldptr3
IL_0016: calli     <string,int&,Module&>
IL_001b: ret       
// (wrapper managed-to-native) System.Type:get_IsGenericTypeDefinition ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:GetGenericArguments ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Type:MakeGenericType (System.Type,System.Type[])
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <Type,Type[]>
IL_000d: ret       
// (wrapper managed-to-native) System.Environment:GetOSVersionString ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Environment:get_Platform ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Runtime.InteropServices.Marshal:copy_from_unmanaged (intptr,int,System.Array,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: mono_ldptr2
IL_000a: calli     <intptr,int,Array,int>
IL_000f: ret       
// (wrapper managed-to-managed) string:.ctor (char*,int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: call      <string:CreateString (char*,int,int)>
IL_0009: ret       
// (wrapper managed-to-native) System.Reflection.Emit.AssemblyBuilder:basic_init (System.Reflection.Emit.AssemblyBuilder)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <AssemblyBuilder>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:basic_init (System.Reflection.Emit.ModuleBuilder)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <ModuleBuilder>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.Emit.TypeBuilder:setup_internal_class (System.Reflection.Emit.TypeBuilder)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <TypeBuilder>
IL_0019: ret       
// (wrapper managed-to-native) System.Reflection.Emit.TypeBuilder:create_generic_class ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Reflection.Emit.TypeBuilder:create_runtime_class (System.Reflection.Emit.TypeBuilder)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <TypeBuilder>
IL_0019: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:set_wrappers_type (System.Reflection.Emit.ModuleBuilder,System.Type)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <ModuleBuilder,Type>
IL_000d: ret       
// (wrapper managed-to-native) System.MonoType:get_Name ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:GetFields_internal (System.Reflection.BindingFlags,System.Type)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <BindingFlags,Type>
IL_001a: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:RegisterToken (object,int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <object,int>
IL_001a: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:getToken (System.Reflection.Emit.ModuleBuilder,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <ModuleBuilder,object>
IL_000d: ret       
// (wrapper managed-to-native) System.MonoType:get_Module ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:GetElementType ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:getUSIndex (System.Reflection.Emit.ModuleBuilder,string)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <ModuleBuilder,string>
IL_000d: ret       
// (wrapper managed-to-native) System.MonoType:GetArrayRank ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Reflection.MonoField:GetParentType (bool)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <bool>
IL_0019: ret       
// TypeInfo__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000001>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// TypeInfo__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// TypeInfo__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    3
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.2   
IL_0017: ldstr     
IL_001c: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0021: ldarg.2   
IL_0022: ldstr     
IL_0027: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002c: ldarg.3   
IL_002d: brfalse   IL_004a
IL_0032: ldarg.2   
IL_0033: ldc.i4.s  1
IL_0035: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_003a: ldarg.2   
IL_003b: ldc.i4.s  6
IL_003d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0042: ldarg.2   
IL_0043: ldc.i4.s  6
IL_0045: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_004a: ret       
// TypeInfo__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <TypeInfo>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x01000005>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Runtime.Remoting.TypeInfo:serverType>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ldarg.1   
IL_001f: ldarg.2   
IL_0020: ldtoken   <0x1b000001>
IL_0025: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_002a: ldloc.0   
IL_002b: ldfld     <System.Runtime.Remoting.TypeInfo:serverHierarchy>
IL_0030: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0035: ldarg.1   
IL_0036: ldarg.2   
IL_0037: ldtoken   <0x1b000001>
IL_003c: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0041: ldloc.0   
IL_0042: ldfld     <System.Runtime.Remoting.TypeInfo:interfacesImplemented>
IL_0047: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_004c: ret       
// (wrapper managed-to-native) System.ValueType:InternalEquals (object,object,object[]&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <object,object,object[]&>
IL_000e: ret       
// ChannelInfo__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000007>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// ChannelInfo__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// ChannelInfo__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    1
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.3   
IL_0017: brfalse   IL_0024
IL_001c: ldarg.2   
IL_001d: ldc.i4.s  5
IL_001f: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0024: ret       
// ChannelInfo__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <ChannelInfo>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x1b000002>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Runtime.Remoting.ChannelInfo:channelData>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ret       
// CrossAppDomainData__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000008>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// CrossAppDomainData__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// CrossAppDomainData__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    3
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.2   
IL_0017: ldstr     
IL_001c: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0021: ldarg.2   
IL_0022: ldstr     
IL_0027: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002c: ldarg.3   
IL_002d: brfalse   IL_0052
IL_0032: ldarg.2   
IL_0033: ldc.i4.s  2
IL_0035: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_003a: ldarg.2   
IL_003b: ldc.i4.s  0
IL_003d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0042: ldarg.2   
IL_0043: ldc.i4.s  1
IL_0045: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_004a: ldarg.2   
IL_004b: ldc.i4.s  8
IL_004d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0052: ret       
// CrossAppDomainData__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <CrossAppDomainData>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x01000009>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Runtime.Remoting.Channels.CrossAppDomainData:_ContextID>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ldarg.2   
IL_001f: ldloc.0   
IL_0020: ldfld     <System.Runtime.Remoting.Channels.CrossAppDomainData:_DomainID>
IL_0025: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_002a: ldarg.1   
IL_002b: ldarg.2   
IL_002c: ldtoken   <0x01000005>
IL_0031: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0036: ldloc.0   
IL_0037: ldfld     <System.Runtime.Remoting.Channels.CrossAppDomainData:_processGuid>
IL_003c: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0041: ret       
// (wrapper managed-to-native) System.AppDomain:InternalSetDomain (System.AppDomain)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <AppDomain>
IL_000c: ret       
// (wrapper managed-to-native) System.AppDomain:InternalPopDomainRef ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.AppDomain:InternalSetContext (System.Runtime.Remoting.Contexts.Context)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Context>
IL_000c: ret       
// (wrapper managed-to-native) System.Runtime.Remoting.Activation.ActivationServices:AllocateUninitializedClassInstance (System.Type)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Type>
IL_000c: ret       
// (wrapper managed-to-native) System.Array:Clone ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:get_BaseType ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.MonoType:IsPrimitiveImpl ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Reflection.MonoField:SetValueInternal (System.Reflection.FieldInfo,object,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <FieldInfo,object,object>
IL_000e: ret       
// (wrapper managed-to-native) System.Array:SetValue (object,int[])
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <object,int[]>
IL_001a: ret       
// (wrapper managed-to-native) System.Type:IsInstanceOfType (object)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <object>
IL_0019: ret       
// (wrapper runtime-invoke) object:runtime_invoke_void__this___object_StreamingContext (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.1   
IL_001c: ldind.ref 
IL_001d: ldarg.1   
IL_001e: ldc.i4.s  8
IL_0020: add       
IL_0021: ldind.i   
IL_0022: ldobj     <0x00000004>
IL_0027: ldarg.3   
IL_0028: calli     <object,StreamingContext>
IL_002d: ldnull    
IL_002e: stloc.0   
IL_002f: leave     IL_004d
IL_0034: pop       
IL_0035: ldarg.2   
IL_0036: ldc.i4.0  
IL_0037: cgt.un    
IL_0039: endfilter 
IL_003b: stloc.1   
IL_003c: ldarg.2   
IL_003d: ldloc.1   
IL_003e: stind.ref 
IL_003f: ldnull    
IL_0040: stloc.0   
IL_0041: ldloc.1   
IL_0042: mono_icall <runtime_invoke_reset_abort>
IL_0048: leave     IL_004d
IL_004d: ldloc.0   
IL_004e: ret       
// (wrapper managed-to-native) System.AppDomain:InternalGetProcessGuid (string)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <string>
IL_000c: ret       
// (wrapper managed-to-native) System.Runtime.Remoting.Proxies.RealProxy:InternalGetTransparentProxy (string)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <string>
IL_0019: ret       
// (wrapper xdomain-invoke) System.AppDomain:ToString ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) System.AppDomain:ToString ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <String>
IL_00b3: ret       
// (wrapper xdomain-invoke) object:GetHashCode ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) object:GetHashCode ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: ret       
// (wrapper xdomain-invoke) object:Finalize ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) object:Finalize ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: dup       
IL_0090: brfalse.s IL_009d
IL_0092: mono_icall <mono_marshal_xdomain_copy_value>
IL_0098: castclass <Byte[]>
IL_009d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a2: pop       
IL_00a3: ret       
// (wrapper xdomain-invoke) object:Equals (object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   7
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   5
IL_0021: ldloc.s   5
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) object:Equals (object)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   5
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.s   4
IL_0056: ldloc.2   
IL_0057: ldloca.s  1
IL_0059: ldloca.s  6
IL_005b: mono_ldptr10
IL_0061: mono_icall <mono_compile_method>
IL_0067: calli     <object,byte[]&,byte[]&>
IL_006c: stloc.3   
IL_006d: ldloc.s   4
IL_006f: ldc.i4.0  
IL_0070: mono_icall <mono_marshal_set_domain_by_id>
IL_0076: pop       
IL_0077: ldloc.s   7
IL_0079: mono_icall <mono_context_set>
IL_007f: ldloc.s   6
IL_0081: brfalse.s IL_00a0
IL_0083: ldloc.s   6
IL_0085: mono_icall <mono_marshal_xdomain_copy_value>
IL_008b: castclass <Byte[]>
IL_0090: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0095: castclass <Exception>
IL_009a: call      <System.Exception:FixRemotingException ()>
IL_009f: throw     
IL_00a0: ldloc.1   
IL_00a1: dup       
IL_00a2: brfalse.s IL_00af
IL_00a4: mono_icall <mono_marshal_xdomain_copy_value>
IL_00aa: castclass <Byte[]>
IL_00af: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b4: pop       
IL_00b5: ldloc.3   
IL_00b6: ret       
// (wrapper xdomain-invoke) System.AppDomain:InitializeLifetimeService ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.AppDomain:InitializeLifetimeService ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: ret       
// (wrapper xdomain-invoke) System.MarshalByRefObject:CreateObjRef (System.Type)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.MarshalByRefObject:CreateObjRef (System.Type)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a4: castclass <Byte[]>
IL_00a9: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00ae: castclass <ObjRef>
IL_00b3: ret       
// (wrapper xdomain-invoke) System.AppDomain:add_AssemblyLoad (System.AssemblyLoadEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:add_AssemblyLoad (System.AssemblyLoadEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:remove_AssemblyLoad (System.AssemblyLoadEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:remove_AssemblyLoad (System.AssemblyLoadEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:add_AssemblyResolve (System.ResolveEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:add_AssemblyResolve (System.ResolveEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:remove_AssemblyResolve (System.ResolveEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:remove_AssemblyResolve (System.ResolveEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:add_DomainUnload (System.EventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:add_DomainUnload (System.EventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:remove_DomainUnload (System.EventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:remove_DomainUnload (System.EventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:add_ProcessExit (System.EventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:add_ProcessExit (System.EventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:remove_ProcessExit (System.EventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:remove_ProcessExit (System.EventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:add_ResourceResolve (System.ResolveEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:add_ResourceResolve (System.ResolveEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:remove_ResourceResolve (System.ResolveEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:remove_ResourceResolve (System.ResolveEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:add_TypeResolve (System.ResolveEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:add_TypeResolve (System.ResolveEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:remove_TypeResolve (System.ResolveEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:remove_TypeResolve (System.ResolveEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:add_UnhandledException (System.UnhandledExceptionEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:add_UnhandledException (System.UnhandledExceptionEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:remove_UnhandledException (System.UnhandledExceptionEventHandler)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:remove_UnhandledException (System.UnhandledExceptionEventHandler)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:get_BaseDirectory ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) System.AppDomain:get_BaseDirectory ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <String>
IL_00b3: ret       
// (wrapper xdomain-invoke) System.AppDomain:get_DynamicDirectory ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) System.AppDomain:get_DynamicDirectory ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <String>
IL_00b3: ret       
// (wrapper xdomain-invoke) System.AppDomain:get_Evidence ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.AppDomain:get_Evidence ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: castclass <Evidence>
IL_00a4: ret       
// (wrapper xdomain-invoke) System.AppDomain:get_FriendlyName ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) System.AppDomain:get_FriendlyName ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <String>
IL_00b3: ret       
// (wrapper xdomain-invoke) System.AppDomain:get_RelativeSearchPath ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) System.AppDomain:get_RelativeSearchPath ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <String>
IL_00b3: ret       
// (wrapper xdomain-invoke) System.AppDomain:get_ShadowCopyFiles ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) System.AppDomain:get_ShadowCopyFiles ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: ret       
// (wrapper xdomain-invoke) System.AppDomain:AppendPrivatePath (string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.AppDomain:AppendPrivatePath (string)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,string>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.AppDomain:ClearPrivatePath ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.AppDomain:ClearPrivatePath ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: dup       
IL_0090: brfalse.s IL_009d
IL_0092: mono_icall <mono_marshal_xdomain_copy_value>
IL_0098: castclass <Byte[]>
IL_009d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a2: pop       
IL_00a3: ret       
// (wrapper xdomain-invoke) System.AppDomain:ClearShadowCopyPath ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.AppDomain:ClearShadowCopyPath ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: dup       
IL_0090: brfalse.s IL_009d
IL_0092: mono_icall <mono_marshal_xdomain_copy_value>
IL_0098: castclass <Byte[]>
IL_009d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a2: pop       
IL_00a3: ret       
// (wrapper xdomain-invoke) System.AppDomain:CreateInstance (string,string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0038
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: ldarg.2   
IL_0032: call      <(wrapper remoting-invoke) System.AppDomain:CreateInstance (string,string)>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.3   
IL_0040: ldc.i4.1  
IL_0041: mono_icall <mono_marshal_set_domain_by_id>
IL_0047: stloc.2   
IL_0048: ldloc.1   
IL_0049: ldloca.s  0
IL_004b: ldloca.s  4
IL_004d: ldarg.1   
IL_004e: ldarg.2   
IL_004f: mono_ldptr9
IL_0055: mono_icall <mono_compile_method>
IL_005b: calli     <object,byte[]&,byte[]&,string,string>
IL_0060: ldloc.2   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   5
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   4
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   4
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: mono_icall <mono_marshal_xdomain_copy_value>
IL_0099: castclass <Byte[]>
IL_009e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a3: castclass <ObjectHandle>
IL_00a8: ret       
// (wrapper xdomain-invoke) System.AppDomain:CreateInstance (string,string,object[])
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003b
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: call      <(wrapper remoting-invoke) System.AppDomain:CreateInstance (string,string,object[])>
IL_003a: ret       
IL_003b: ldc.i4.1  
IL_003c: newarr    <0x00000007>
IL_0041: dup       
IL_0042: ldc.i4.0  
IL_0043: ldarg.3   
IL_0044: stelem.ref
IL_0045: stloc.0   
IL_0046: ldloc.0   
IL_0047: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004c: stloc.1   
IL_004d: ldloc.s   4
IL_004f: ldc.i4.1  
IL_0050: mono_icall <mono_marshal_set_domain_by_id>
IL_0056: stloc.3   
IL_0057: ldloc.2   
IL_0058: ldloca.s  1
IL_005a: ldloca.s  5
IL_005c: ldarg.1   
IL_005d: ldarg.2   
IL_005e: mono_ldptr10
IL_0064: mono_icall <mono_compile_method>
IL_006a: calli     <object,byte[]&,byte[]&,string,string>
IL_006f: ldloc.3   
IL_0070: ldc.i4.0  
IL_0071: mono_icall <mono_marshal_set_domain_by_id>
IL_0077: pop       
IL_0078: ldloc.s   6
IL_007a: mono_icall <mono_context_set>
IL_0080: ldloc.s   5
IL_0082: brfalse.s IL_00a1
IL_0084: ldloc.s   5
IL_0086: mono_icall <mono_marshal_xdomain_copy_value>
IL_008c: castclass <Byte[]>
IL_0091: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0096: castclass <Exception>
IL_009b: call      <System.Exception:FixRemotingException ()>
IL_00a0: throw     
IL_00a1: ldloc.1   
IL_00a2: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a8: castclass <Byte[]>
IL_00ad: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b2: castclass <ObjectHandle>
IL_00b7: ret       
// (wrapper xdomain-invoke) System.AppDomain:CreateInstance (string,string,bool,System.Reflection.BindingFlags,System.Reflection.Binder,object[],System.Globalization.CultureInfo,object[],System.Security.Policy.Evidence)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0047
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: ldarg.s   5
IL_0039: ldarg.s   6
IL_003b: ldarg.s   7
IL_003d: ldarg.s   8
IL_003f: ldarg.s   9
IL_0041: call      <(wrapper remoting-invoke) System.AppDomain:CreateInstance (string,string,bool,System.Reflection.BindingFlags,System.Reflection.Binder,object[],System.Globalization.CultureInfo,object[],System.Security.Policy.Evidence)>
IL_0046: ret       
IL_0047: ldc.i4.6  
IL_0048: newarr    <0x00000007>
IL_004d: dup       
IL_004e: ldc.i4.0  
IL_004f: ldarg.s   4
IL_0051: box       <0x00000008>
IL_0056: stelem.ref
IL_0057: dup       
IL_0058: ldc.i4.1  
IL_0059: ldarg.s   5
IL_005b: stelem.ref
IL_005c: dup       
IL_005d: ldc.i4.2  
IL_005e: ldarg.s   6
IL_0060: stelem.ref
IL_0061: dup       
IL_0062: ldc.i4.3  
IL_0063: ldarg.s   7
IL_0065: stelem.ref
IL_0066: dup       
IL_0067: ldc.i4.4  
IL_0068: ldarg.s   8
IL_006a: stelem.ref
IL_006b: dup       
IL_006c: ldc.i4.5  
IL_006d: ldarg.s   9
IL_006f: stelem.ref
IL_0070: stloc.0   
IL_0071: ldloc.0   
IL_0072: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0077: stloc.1   
IL_0078: ldloc.s   4
IL_007a: ldc.i4.1  
IL_007b: mono_icall <mono_marshal_set_domain_by_id>
IL_0081: stloc.3   
IL_0082: ldloc.2   
IL_0083: ldloca.s  1
IL_0085: ldloca.s  5
IL_0087: ldarg.1   
IL_0088: ldarg.2   
IL_0089: ldarg.3   
IL_008a: mono_ldptr11
IL_0090: mono_icall <mono_compile_method>
IL_0096: calli     <object,byte[]&,byte[]&,string,string,bool>
IL_009b: ldloc.3   
IL_009c: ldc.i4.0  
IL_009d: mono_icall <mono_marshal_set_domain_by_id>
IL_00a3: pop       
IL_00a4: ldloc.s   6
IL_00a6: mono_icall <mono_context_set>
IL_00ac: ldloc.s   5
IL_00ae: brfalse.s IL_00cd
IL_00b0: ldloc.s   5
IL_00b2: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b8: castclass <Byte[]>
IL_00bd: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00c2: castclass <Exception>
IL_00c7: call      <System.Exception:FixRemotingException ()>
IL_00cc: throw     
IL_00cd: ldloc.1   
IL_00ce: mono_icall <mono_marshal_xdomain_copy_value>
IL_00d4: castclass <Byte[]>
IL_00d9: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00de: castclass <ObjectHandle>
IL_00e3: ret       
// (wrapper xdomain-invoke) System.AppDomain:CreateInstanceFrom (string,string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0038
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: ldarg.2   
IL_0032: call      <(wrapper remoting-invoke) System.AppDomain:CreateInstanceFrom (string,string)>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.3   
IL_0040: ldc.i4.1  
IL_0041: mono_icall <mono_marshal_set_domain_by_id>
IL_0047: stloc.2   
IL_0048: ldloc.1   
IL_0049: ldloca.s  0
IL_004b: ldloca.s  4
IL_004d: ldarg.1   
IL_004e: ldarg.2   
IL_004f: mono_ldptr9
IL_0055: mono_icall <mono_compile_method>
IL_005b: calli     <object,byte[]&,byte[]&,string,string>
IL_0060: ldloc.2   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   5
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   4
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   4
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: mono_icall <mono_marshal_xdomain_copy_value>
IL_0099: castclass <Byte[]>
IL_009e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a3: castclass <ObjectHandle>
IL_00a8: ret       
// (wrapper xdomain-invoke) System.AppDomain:CreateInstanceFrom (string,string,object[])
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003b
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: call      <(wrapper remoting-invoke) System.AppDomain:CreateInstanceFrom (string,string,object[])>
IL_003a: ret       
IL_003b: ldc.i4.1  
IL_003c: newarr    <0x00000007>
IL_0041: dup       
IL_0042: ldc.i4.0  
IL_0043: ldarg.3   
IL_0044: stelem.ref
IL_0045: stloc.0   
IL_0046: ldloc.0   
IL_0047: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004c: stloc.1   
IL_004d: ldloc.s   4
IL_004f: ldc.i4.1  
IL_0050: mono_icall <mono_marshal_set_domain_by_id>
IL_0056: stloc.3   
IL_0057: ldloc.2   
IL_0058: ldloca.s  1
IL_005a: ldloca.s  5
IL_005c: ldarg.1   
IL_005d: ldarg.2   
IL_005e: mono_ldptr10
IL_0064: mono_icall <mono_compile_method>
IL_006a: calli     <object,byte[]&,byte[]&,string,string>
IL_006f: ldloc.3   
IL_0070: ldc.i4.0  
IL_0071: mono_icall <mono_marshal_set_domain_by_id>
IL_0077: pop       
IL_0078: ldloc.s   6
IL_007a: mono_icall <mono_context_set>
IL_0080: ldloc.s   5
IL_0082: brfalse.s IL_00a1
IL_0084: ldloc.s   5
IL_0086: mono_icall <mono_marshal_xdomain_copy_value>
IL_008c: castclass <Byte[]>
IL_0091: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0096: castclass <Exception>
IL_009b: call      <System.Exception:FixRemotingException ()>
IL_00a0: throw     
IL_00a1: ldloc.1   
IL_00a2: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a8: castclass <Byte[]>
IL_00ad: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b2: castclass <ObjectHandle>
IL_00b7: ret       
// (wrapper xdomain-invoke) System.AppDomain:CreateInstanceFrom (string,string,bool,System.Reflection.BindingFlags,System.Reflection.Binder,object[],System.Globalization.CultureInfo,object[],System.Security.Policy.Evidence)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0047
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: ldarg.s   5
IL_0039: ldarg.s   6
IL_003b: ldarg.s   7
IL_003d: ldarg.s   8
IL_003f: ldarg.s   9
IL_0041: call      <(wrapper remoting-invoke) System.AppDomain:CreateInstanceFrom (string,string,bool,System.Reflection.BindingFlags,System.Reflection.Binder,object[],System.Globalization.CultureInfo,object[],System.Security.Policy.Evidence)>
IL_0046: ret       
IL_0047: ldc.i4.6  
IL_0048: newarr    <0x00000007>
IL_004d: dup       
IL_004e: ldc.i4.0  
IL_004f: ldarg.s   4
IL_0051: box       <0x00000008>
IL_0056: stelem.ref
IL_0057: dup       
IL_0058: ldc.i4.1  
IL_0059: ldarg.s   5
IL_005b: stelem.ref
IL_005c: dup       
IL_005d: ldc.i4.2  
IL_005e: ldarg.s   6
IL_0060: stelem.ref
IL_0061: dup       
IL_0062: ldc.i4.3  
IL_0063: ldarg.s   7
IL_0065: stelem.ref
IL_0066: dup       
IL_0067: ldc.i4.4  
IL_0068: ldarg.s   8
IL_006a: stelem.ref
IL_006b: dup       
IL_006c: ldc.i4.5  
IL_006d: ldarg.s   9
IL_006f: stelem.ref
IL_0070: stloc.0   
IL_0071: ldloc.0   
IL_0072: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0077: stloc.1   
IL_0078: ldloc.s   4
IL_007a: ldc.i4.1  
IL_007b: mono_icall <mono_marshal_set_domain_by_id>
IL_0081: stloc.3   
IL_0082: ldloc.2   
IL_0083: ldloca.s  1
IL_0085: ldloca.s  5
IL_0087: ldarg.1   
IL_0088: ldarg.2   
IL_0089: ldarg.3   
IL_008a: mono_ldptr11
IL_0090: mono_icall <mono_compile_method>
IL_0096: calli     <object,byte[]&,byte[]&,string,string,bool>
IL_009b: ldloc.3   
IL_009c: ldc.i4.0  
IL_009d: mono_icall <mono_marshal_set_domain_by_id>
IL_00a3: pop       
IL_00a4: ldloc.s   6
IL_00a6: mono_icall <mono_context_set>
IL_00ac: ldloc.s   5
IL_00ae: brfalse.s IL_00cd
IL_00b0: ldloc.s   5
IL_00b2: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b8: castclass <Byte[]>
IL_00bd: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00c2: castclass <Exception>
IL_00c7: call      <System.Exception:FixRemotingException ()>
IL_00cc: throw     
IL_00cd: ldloc.1   
IL_00ce: mono_icall <mono_marshal_xdomain_copy_value>
IL_00d4: castclass <Byte[]>
IL_00d9: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00de: castclass <ObjectHandle>
IL_00e3: ret       
// (wrapper xdomain-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003a
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: call      <(wrapper remoting-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)>
IL_0039: ret       
IL_003a: ldc.i4.2  
IL_003b: newarr    <0x00000007>
IL_0040: dup       
IL_0041: ldc.i4.0  
IL_0042: ldarg.1   
IL_0043: stelem.ref
IL_0044: dup       
IL_0045: ldc.i4.1  
IL_0046: ldarg.2   
IL_0047: box       <0x00000008>
IL_004c: stelem.ref
IL_004d: stloc.0   
IL_004e: ldloc.0   
IL_004f: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0054: stloc.1   
IL_0055: ldloc.s   4
IL_0057: ldc.i4.1  
IL_0058: mono_icall <mono_marshal_set_domain_by_id>
IL_005e: stloc.3   
IL_005f: ldloc.2   
IL_0060: ldloca.s  1
IL_0062: ldloca.s  5
IL_0064: mono_ldptr11
IL_006a: mono_icall <mono_compile_method>
IL_0070: calli     <object,byte[]&,byte[]&>
IL_0075: ldloc.3   
IL_0076: ldc.i4.0  
IL_0077: mono_icall <mono_marshal_set_domain_by_id>
IL_007d: pop       
IL_007e: ldloc.s   6
IL_0080: mono_icall <mono_context_set>
IL_0086: ldloc.s   5
IL_0088: brfalse.s IL_00a7
IL_008a: ldloc.s   5
IL_008c: mono_icall <mono_marshal_xdomain_copy_value>
IL_0092: castclass <Byte[]>
IL_0097: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009c: castclass <Exception>
IL_00a1: call      <System.Exception:FixRemotingException ()>
IL_00a6: throw     
IL_00a7: ldloc.1   
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <Byte[]>
IL_00b3: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b8: castclass <AssemblyBuilder>
IL_00bd: ret       
// (wrapper xdomain-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,System.Security.Policy.Evidence)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003b
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: call      <(wrapper remoting-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,System.Security.Policy.Evidence)>
IL_003a: ret       
IL_003b: ldc.i4.3  
IL_003c: newarr    <0x00000007>
IL_0041: dup       
IL_0042: ldc.i4.0  
IL_0043: ldarg.1   
IL_0044: stelem.ref
IL_0045: dup       
IL_0046: ldc.i4.1  
IL_0047: ldarg.2   
IL_0048: box       <0x00000008>
IL_004d: stelem.ref
IL_004e: dup       
IL_004f: ldc.i4.2  
IL_0050: ldarg.3   
IL_0051: stelem.ref
IL_0052: stloc.0   
IL_0053: ldloc.0   
IL_0054: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0059: stloc.1   
IL_005a: ldloc.s   4
IL_005c: ldc.i4.1  
IL_005d: mono_icall <mono_marshal_set_domain_by_id>
IL_0063: stloc.3   
IL_0064: ldloc.2   
IL_0065: ldloca.s  1
IL_0067: ldloca.s  5
IL_0069: mono_ldptr11
IL_006f: mono_icall <mono_compile_method>
IL_0075: calli     <object,byte[]&,byte[]&>
IL_007a: ldloc.3   
IL_007b: ldc.i4.0  
IL_007c: mono_icall <mono_marshal_set_domain_by_id>
IL_0082: pop       
IL_0083: ldloc.s   6
IL_0085: mono_icall <mono_context_set>
IL_008b: ldloc.s   5
IL_008d: brfalse.s IL_00ac
IL_008f: ldloc.s   5
IL_0091: mono_icall <mono_marshal_xdomain_copy_value>
IL_0097: castclass <Byte[]>
IL_009c: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a1: castclass <Exception>
IL_00a6: call      <System.Exception:FixRemotingException ()>
IL_00ab: throw     
IL_00ac: ldloc.1   
IL_00ad: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b3: castclass <Byte[]>
IL_00b8: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00bd: castclass <AssemblyBuilder>
IL_00c2: ret       
// (wrapper xdomain-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003b
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: call      <(wrapper remoting-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string)>
IL_003a: ret       
IL_003b: ldc.i4.2  
IL_003c: newarr    <0x00000007>
IL_0041: dup       
IL_0042: ldc.i4.0  
IL_0043: ldarg.1   
IL_0044: stelem.ref
IL_0045: dup       
IL_0046: ldc.i4.1  
IL_0047: ldarg.2   
IL_0048: box       <0x00000008>
IL_004d: stelem.ref
IL_004e: stloc.0   
IL_004f: ldloc.0   
IL_0050: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0055: stloc.1   
IL_0056: ldloc.s   4
IL_0058: ldc.i4.1  
IL_0059: mono_icall <mono_marshal_set_domain_by_id>
IL_005f: stloc.3   
IL_0060: ldloc.2   
IL_0061: ldloca.s  1
IL_0063: ldloca.s  5
IL_0065: ldarg.3   
IL_0066: mono_ldptr11
IL_006c: mono_icall <mono_compile_method>
IL_0072: calli     <object,byte[]&,byte[]&,string>
IL_0077: ldloc.3   
IL_0078: ldc.i4.0  
IL_0079: mono_icall <mono_marshal_set_domain_by_id>
IL_007f: pop       
IL_0080: ldloc.s   6
IL_0082: mono_icall <mono_context_set>
IL_0088: ldloc.s   5
IL_008a: brfalse.s IL_00a9
IL_008c: ldloc.s   5
IL_008e: mono_icall <mono_marshal_xdomain_copy_value>
IL_0094: castclass <Byte[]>
IL_0099: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009e: castclass <Exception>
IL_00a3: call      <System.Exception:FixRemotingException ()>
IL_00a8: throw     
IL_00a9: ldloc.1   
IL_00aa: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b0: castclass <Byte[]>
IL_00b5: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00ba: castclass <AssemblyBuilder>
IL_00bf: ret       
// (wrapper xdomain-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string,System.Security.Policy.Evidence)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003d
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: call      <(wrapper remoting-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string,System.Security.Policy.Evidence)>
IL_003c: ret       
IL_003d: ldc.i4.3  
IL_003e: newarr    <0x00000007>
IL_0043: dup       
IL_0044: ldc.i4.0  
IL_0045: ldarg.1   
IL_0046: stelem.ref
IL_0047: dup       
IL_0048: ldc.i4.1  
IL_0049: ldarg.2   
IL_004a: box       <0x00000008>
IL_004f: stelem.ref
IL_0050: dup       
IL_0051: ldc.i4.2  
IL_0052: ldarg.s   4
IL_0054: stelem.ref
IL_0055: stloc.0   
IL_0056: ldloc.0   
IL_0057: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_005c: stloc.1   
IL_005d: ldloc.s   4
IL_005f: ldc.i4.1  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: stloc.3   
IL_0067: ldloc.2   
IL_0068: ldloca.s  1
IL_006a: ldloca.s  5
IL_006c: ldarg.3   
IL_006d: mono_ldptr11
IL_0073: mono_icall <mono_compile_method>
IL_0079: calli     <object,byte[]&,byte[]&,string>
IL_007e: ldloc.3   
IL_007f: ldc.i4.0  
IL_0080: mono_icall <mono_marshal_set_domain_by_id>
IL_0086: pop       
IL_0087: ldloc.s   6
IL_0089: mono_icall <mono_context_set>
IL_008f: ldloc.s   5
IL_0091: brfalse.s IL_00b0
IL_0093: ldloc.s   5
IL_0095: mono_icall <mono_marshal_xdomain_copy_value>
IL_009b: castclass <Byte[]>
IL_00a0: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a5: castclass <Exception>
IL_00aa: call      <System.Exception:FixRemotingException ()>
IL_00af: throw     
IL_00b0: ldloc.1   
IL_00b1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b7: castclass <Byte[]>
IL_00bc: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00c1: castclass <AssemblyBuilder>
IL_00c6: ret       
// (wrapper xdomain-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003f
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: ldarg.s   5
IL_0039: call      <(wrapper remoting-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet)>
IL_003e: ret       
IL_003f: ldc.i4.5  
IL_0040: newarr    <0x00000007>
IL_0045: dup       
IL_0046: ldc.i4.0  
IL_0047: ldarg.1   
IL_0048: stelem.ref
IL_0049: dup       
IL_004a: ldc.i4.1  
IL_004b: ldarg.2   
IL_004c: box       <0x00000008>
IL_0051: stelem.ref
IL_0052: dup       
IL_0053: ldc.i4.2  
IL_0054: ldarg.3   
IL_0055: stelem.ref
IL_0056: dup       
IL_0057: ldc.i4.3  
IL_0058: ldarg.s   4
IL_005a: stelem.ref
IL_005b: dup       
IL_005c: ldc.i4.4  
IL_005d: ldarg.s   5
IL_005f: stelem.ref
IL_0060: stloc.0   
IL_0061: ldloc.0   
IL_0062: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0067: stloc.1   
IL_0068: ldloc.s   4
IL_006a: ldc.i4.1  
IL_006b: mono_icall <mono_marshal_set_domain_by_id>
IL_0071: stloc.3   
IL_0072: ldloc.2   
IL_0073: ldloca.s  1
IL_0075: ldloca.s  5
IL_0077: mono_ldptr11
IL_007d: mono_icall <mono_compile_method>
IL_0083: calli     <object,byte[]&,byte[]&>
IL_0088: ldloc.3   
IL_0089: ldc.i4.0  
IL_008a: mono_icall <mono_marshal_set_domain_by_id>
IL_0090: pop       
IL_0091: ldloc.s   6
IL_0093: mono_icall <mono_context_set>
IL_0099: ldloc.s   5
IL_009b: brfalse.s IL_00ba
IL_009d: ldloc.s   5
IL_009f: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a5: castclass <Byte[]>
IL_00aa: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00af: castclass <Exception>
IL_00b4: call      <System.Exception:FixRemotingException ()>
IL_00b9: throw     
IL_00ba: ldloc.1   
IL_00bb: mono_icall <mono_marshal_xdomain_copy_value>
IL_00c1: castclass <Byte[]>
IL_00c6: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00cb: castclass <AssemblyBuilder>
IL_00d0: ret       
// (wrapper xdomain-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,System.Security.Policy.Evidence,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0041
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: ldarg.s   5
IL_0039: ldarg.s   6
IL_003b: call      <(wrapper remoting-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,System.Security.Policy.Evidence,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet)>
IL_0040: ret       
IL_0041: ldc.i4.6  
IL_0042: newarr    <0x00000007>
IL_0047: dup       
IL_0048: ldc.i4.0  
IL_0049: ldarg.1   
IL_004a: stelem.ref
IL_004b: dup       
IL_004c: ldc.i4.1  
IL_004d: ldarg.2   
IL_004e: box       <0x00000008>
IL_0053: stelem.ref
IL_0054: dup       
IL_0055: ldc.i4.2  
IL_0056: ldarg.3   
IL_0057: stelem.ref
IL_0058: dup       
IL_0059: ldc.i4.3  
IL_005a: ldarg.s   4
IL_005c: stelem.ref
IL_005d: dup       
IL_005e: ldc.i4.4  
IL_005f: ldarg.s   5
IL_0061: stelem.ref
IL_0062: dup       
IL_0063: ldc.i4.5  
IL_0064: ldarg.s   6
IL_0066: stelem.ref
IL_0067: stloc.0   
IL_0068: ldloc.0   
IL_0069: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_006e: stloc.1   
IL_006f: ldloc.s   4
IL_0071: ldc.i4.1  
IL_0072: mono_icall <mono_marshal_set_domain_by_id>
IL_0078: stloc.3   
IL_0079: ldloc.2   
IL_007a: ldloca.s  1
IL_007c: ldloca.s  5
IL_007e: mono_ldptr11
IL_0084: mono_icall <mono_compile_method>
IL_008a: calli     <object,byte[]&,byte[]&>
IL_008f: ldloc.3   
IL_0090: ldc.i4.0  
IL_0091: mono_icall <mono_marshal_set_domain_by_id>
IL_0097: pop       
IL_0098: ldloc.s   6
IL_009a: mono_icall <mono_context_set>
IL_00a0: ldloc.s   5
IL_00a2: brfalse.s IL_00c1
IL_00a4: ldloc.s   5
IL_00a6: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ac: castclass <Byte[]>
IL_00b1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b6: castclass <Exception>
IL_00bb: call      <System.Exception:FixRemotingException ()>
IL_00c0: throw     
IL_00c1: ldloc.1   
IL_00c2: mono_icall <mono_marshal_xdomain_copy_value>
IL_00c8: castclass <Byte[]>
IL_00cd: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00d2: castclass <AssemblyBuilder>
IL_00d7: ret       
// (wrapper xdomain-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0041
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: ldarg.s   5
IL_0039: ldarg.s   6
IL_003b: call      <(wrapper remoting-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet)>
IL_0040: ret       
IL_0041: ldc.i4.5  
IL_0042: newarr    <0x00000007>
IL_0047: dup       
IL_0048: ldc.i4.0  
IL_0049: ldarg.1   
IL_004a: stelem.ref
IL_004b: dup       
IL_004c: ldc.i4.1  
IL_004d: ldarg.2   
IL_004e: box       <0x00000008>
IL_0053: stelem.ref
IL_0054: dup       
IL_0055: ldc.i4.2  
IL_0056: ldarg.s   4
IL_0058: stelem.ref
IL_0059: dup       
IL_005a: ldc.i4.3  
IL_005b: ldarg.s   5
IL_005d: stelem.ref
IL_005e: dup       
IL_005f: ldc.i4.4  
IL_0060: ldarg.s   6
IL_0062: stelem.ref
IL_0063: stloc.0   
IL_0064: ldloc.0   
IL_0065: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_006a: stloc.1   
IL_006b: ldloc.s   4
IL_006d: ldc.i4.1  
IL_006e: mono_icall <mono_marshal_set_domain_by_id>
IL_0074: stloc.3   
IL_0075: ldloc.2   
IL_0076: ldloca.s  1
IL_0078: ldloca.s  5
IL_007a: ldarg.3   
IL_007b: mono_ldptr11
IL_0081: mono_icall <mono_compile_method>
IL_0087: calli     <object,byte[]&,byte[]&,string>
IL_008c: ldloc.3   
IL_008d: ldc.i4.0  
IL_008e: mono_icall <mono_marshal_set_domain_by_id>
IL_0094: pop       
IL_0095: ldloc.s   6
IL_0097: mono_icall <mono_context_set>
IL_009d: ldloc.s   5
IL_009f: brfalse.s IL_00be
IL_00a1: ldloc.s   5
IL_00a3: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a9: castclass <Byte[]>
IL_00ae: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b3: castclass <Exception>
IL_00b8: call      <System.Exception:FixRemotingException ()>
IL_00bd: throw     
IL_00be: ldloc.1   
IL_00bf: mono_icall <mono_marshal_xdomain_copy_value>
IL_00c5: castclass <Byte[]>
IL_00ca: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00cf: castclass <AssemblyBuilder>
IL_00d4: ret       
// (wrapper xdomain-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string,System.Security.Policy.Evidence,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0043
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: ldarg.s   5
IL_0039: ldarg.s   6
IL_003b: ldarg.s   7
IL_003d: call      <(wrapper remoting-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string,System.Security.Policy.Evidence,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet)>
IL_0042: ret       
IL_0043: ldc.i4.6  
IL_0044: newarr    <0x00000007>
IL_0049: dup       
IL_004a: ldc.i4.0  
IL_004b: ldarg.1   
IL_004c: stelem.ref
IL_004d: dup       
IL_004e: ldc.i4.1  
IL_004f: ldarg.2   
IL_0050: box       <0x00000008>
IL_0055: stelem.ref
IL_0056: dup       
IL_0057: ldc.i4.2  
IL_0058: ldarg.s   4
IL_005a: stelem.ref
IL_005b: dup       
IL_005c: ldc.i4.3  
IL_005d: ldarg.s   5
IL_005f: stelem.ref
IL_0060: dup       
IL_0061: ldc.i4.4  
IL_0062: ldarg.s   6
IL_0064: stelem.ref
IL_0065: dup       
IL_0066: ldc.i4.5  
IL_0067: ldarg.s   7
IL_0069: stelem.ref
IL_006a: stloc.0   
IL_006b: ldloc.0   
IL_006c: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0071: stloc.1   
IL_0072: ldloc.s   4
IL_0074: ldc.i4.1  
IL_0075: mono_icall <mono_marshal_set_domain_by_id>
IL_007b: stloc.3   
IL_007c: ldloc.2   
IL_007d: ldloca.s  1
IL_007f: ldloca.s  5
IL_0081: ldarg.3   
IL_0082: mono_ldptr11
IL_0088: mono_icall <mono_compile_method>
IL_008e: calli     <object,byte[]&,byte[]&,string>
IL_0093: ldloc.3   
IL_0094: ldc.i4.0  
IL_0095: mono_icall <mono_marshal_set_domain_by_id>
IL_009b: pop       
IL_009c: ldloc.s   6
IL_009e: mono_icall <mono_context_set>
IL_00a4: ldloc.s   5
IL_00a6: brfalse.s IL_00c5
IL_00a8: ldloc.s   5
IL_00aa: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b0: castclass <Byte[]>
IL_00b5: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00ba: castclass <Exception>
IL_00bf: call      <System.Exception:FixRemotingException ()>
IL_00c4: throw     
IL_00c5: ldloc.1   
IL_00c6: mono_icall <mono_marshal_xdomain_copy_value>
IL_00cc: castclass <Byte[]>
IL_00d1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00d6: castclass <AssemblyBuilder>
IL_00db: ret       
// (wrapper xdomain-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string,System.Security.Policy.Evidence,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet,bool)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0045
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: ldarg.s   5
IL_0039: ldarg.s   6
IL_003b: ldarg.s   7
IL_003d: ldarg.s   8
IL_003f: call      <(wrapper remoting-invoke) System.AppDomain:DefineDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess,string,System.Security.Policy.Evidence,System.Security.PermissionSet,System.Security.PermissionSet,System.Security.PermissionSet,bool)>
IL_0044: ret       
IL_0045: ldc.i4.6  
IL_0046: newarr    <0x00000007>
IL_004b: dup       
IL_004c: ldc.i4.0  
IL_004d: ldarg.1   
IL_004e: stelem.ref
IL_004f: dup       
IL_0050: ldc.i4.1  
IL_0051: ldarg.2   
IL_0052: box       <0x00000008>
IL_0057: stelem.ref
IL_0058: dup       
IL_0059: ldc.i4.2  
IL_005a: ldarg.s   4
IL_005c: stelem.ref
IL_005d: dup       
IL_005e: ldc.i4.3  
IL_005f: ldarg.s   5
IL_0061: stelem.ref
IL_0062: dup       
IL_0063: ldc.i4.4  
IL_0064: ldarg.s   6
IL_0066: stelem.ref
IL_0067: dup       
IL_0068: ldc.i4.5  
IL_0069: ldarg.s   7
IL_006b: stelem.ref
IL_006c: stloc.0   
IL_006d: ldloc.0   
IL_006e: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0073: stloc.1   
IL_0074: ldloc.s   4
IL_0076: ldc.i4.1  
IL_0077: mono_icall <mono_marshal_set_domain_by_id>
IL_007d: stloc.3   
IL_007e: ldloc.2   
IL_007f: ldloca.s  1
IL_0081: ldloca.s  5
IL_0083: ldarg.3   
IL_0084: ldarg.s   8
IL_0086: mono_ldptr11
IL_008c: mono_icall <mono_compile_method>
IL_0092: calli     <object,byte[]&,byte[]&,string,bool>
IL_0097: ldloc.3   
IL_0098: ldc.i4.0  
IL_0099: mono_icall <mono_marshal_set_domain_by_id>
IL_009f: pop       
IL_00a0: ldloc.s   6
IL_00a2: mono_icall <mono_context_set>
IL_00a8: ldloc.s   5
IL_00aa: brfalse.s IL_00c9
IL_00ac: ldloc.s   5
IL_00ae: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b4: castclass <Byte[]>
IL_00b9: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00be: castclass <Exception>
IL_00c3: call      <System.Exception:FixRemotingException ()>
IL_00c8: throw     
IL_00c9: ldloc.1   
IL_00ca: mono_icall <mono_marshal_xdomain_copy_value>
IL_00d0: castclass <Byte[]>
IL_00d5: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00da: castclass <AssemblyBuilder>
IL_00df: ret       
// (wrapper xdomain-invoke) System.AppDomain:DoCallBack (System.CrossAppDomainDelegate)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:DoCallBack (System.CrossAppDomainDelegate)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:ExecuteAssembly (string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:ExecuteAssembly (string)>
IL_0038: ret       
IL_0039: ldnull    
IL_003a: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003f: stloc.0   
IL_0040: ldloc.s   4
IL_0042: ldc.i4.1  
IL_0043: mono_icall <mono_marshal_set_domain_by_id>
IL_0049: stloc.3   
IL_004a: ldloc.1   
IL_004b: ldloca.s  0
IL_004d: ldloca.s  5
IL_004f: ldarg.1   
IL_0050: mono_ldptr9
IL_0056: mono_icall <mono_compile_method>
IL_005c: calli     <object,byte[]&,byte[]&,string>
IL_0061: stloc.2   
IL_0062: ldloc.3   
IL_0063: ldc.i4.0  
IL_0064: mono_icall <mono_marshal_set_domain_by_id>
IL_006a: pop       
IL_006b: ldloc.s   6
IL_006d: mono_icall <mono_context_set>
IL_0073: ldloc.s   5
IL_0075: brfalse.s IL_0094
IL_0077: ldloc.s   5
IL_0079: mono_icall <mono_marshal_xdomain_copy_value>
IL_007f: castclass <Byte[]>
IL_0084: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0089: castclass <Exception>
IL_008e: call      <System.Exception:FixRemotingException ()>
IL_0093: throw     
IL_0094: ldloc.0   
IL_0095: dup       
IL_0096: brfalse.s IL_00a3
IL_0098: mono_icall <mono_marshal_xdomain_copy_value>
IL_009e: castclass <Byte[]>
IL_00a3: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a8: pop       
IL_00a9: ldloc.2   
IL_00aa: ret       
// (wrapper xdomain-invoke) System.AppDomain:ExecuteAssembly (string,System.Security.Policy.Evidence)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   7
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   5
IL_0021: ldloc.s   5
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003a
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: call      <(wrapper remoting-invoke) System.AppDomain:ExecuteAssembly (string,System.Security.Policy.Evidence)>
IL_0039: ret       
IL_003a: ldc.i4.1  
IL_003b: newarr    <0x00000007>
IL_0040: dup       
IL_0041: ldc.i4.0  
IL_0042: ldarg.2   
IL_0043: stelem.ref
IL_0044: stloc.0   
IL_0045: ldloc.0   
IL_0046: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004b: stloc.1   
IL_004c: ldloc.s   5
IL_004e: ldc.i4.1  
IL_004f: mono_icall <mono_marshal_set_domain_by_id>
IL_0055: stloc.s   4
IL_0057: ldloc.2   
IL_0058: ldloca.s  1
IL_005a: ldloca.s  6
IL_005c: ldarg.1   
IL_005d: mono_ldptr10
IL_0063: mono_icall <mono_compile_method>
IL_0069: calli     <object,byte[]&,byte[]&,string>
IL_006e: stloc.3   
IL_006f: ldloc.s   4
IL_0071: ldc.i4.0  
IL_0072: mono_icall <mono_marshal_set_domain_by_id>
IL_0078: pop       
IL_0079: ldloc.s   7
IL_007b: mono_icall <mono_context_set>
IL_0081: ldloc.s   6
IL_0083: brfalse.s IL_00a2
IL_0085: ldloc.s   6
IL_0087: mono_icall <mono_marshal_xdomain_copy_value>
IL_008d: castclass <Byte[]>
IL_0092: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0097: castclass <Exception>
IL_009c: call      <System.Exception:FixRemotingException ()>
IL_00a1: throw     
IL_00a2: ldloc.1   
IL_00a3: dup       
IL_00a4: brfalse.s IL_00b1
IL_00a6: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ac: castclass <Byte[]>
IL_00b1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b6: pop       
IL_00b7: ldloc.3   
IL_00b8: ret       
// (wrapper xdomain-invoke) System.AppDomain:ExecuteAssembly (string,System.Security.Policy.Evidence,string[])
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   7
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   5
IL_0021: ldloc.s   5
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003b
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: call      <(wrapper remoting-invoke) System.AppDomain:ExecuteAssembly (string,System.Security.Policy.Evidence,string[])>
IL_003a: ret       
IL_003b: ldc.i4.1  
IL_003c: newarr    <0x00000007>
IL_0041: dup       
IL_0042: ldc.i4.0  
IL_0043: ldarg.2   
IL_0044: stelem.ref
IL_0045: stloc.0   
IL_0046: ldloc.0   
IL_0047: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004c: stloc.1   
IL_004d: ldloc.s   5
IL_004f: ldc.i4.1  
IL_0050: mono_icall <mono_marshal_set_domain_by_id>
IL_0056: stloc.s   4
IL_0058: ldloc.2   
IL_0059: ldloca.s  1
IL_005b: ldloca.s  6
IL_005d: ldarg.1   
IL_005e: ldarg.3   
IL_005f: mono_ldptr10
IL_0065: mono_icall <mono_compile_method>
IL_006b: calli     <object,byte[]&,byte[]&,string,string[]>
IL_0070: stloc.3   
IL_0071: ldloc.s   4
IL_0073: ldc.i4.0  
IL_0074: mono_icall <mono_marshal_set_domain_by_id>
IL_007a: pop       
IL_007b: ldloc.s   7
IL_007d: mono_icall <mono_context_set>
IL_0083: ldloc.s   6
IL_0085: brfalse.s IL_00a4
IL_0087: ldloc.s   6
IL_0089: mono_icall <mono_marshal_xdomain_copy_value>
IL_008f: castclass <Byte[]>
IL_0094: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0099: castclass <Exception>
IL_009e: call      <System.Exception:FixRemotingException ()>
IL_00a3: throw     
IL_00a4: ldloc.1   
IL_00a5: dup       
IL_00a6: brfalse.s IL_00b3
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <Byte[]>
IL_00b3: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b8: pop       
IL_00b9: ldloc.3   
IL_00ba: ret       
// (wrapper xdomain-invoke) System.AppDomain:GetAssemblies ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.AppDomain:GetAssemblies ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: castclass <Assembly[]>
IL_00a4: ret       
// (wrapper xdomain-invoke) System.AppDomain:GetData (string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.AppDomain:GetData (string)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,string>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: mono_icall <mono_marshal_xdomain_copy_value>
IL_0097: castclass <Byte[]>
IL_009c: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a1: ret       
// (wrapper xdomain-invoke) System.AppDomain:System._AppDomain.GetLifetimeService ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.AppDomain:System._AppDomain.GetLifetimeService ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: ret       
// (wrapper xdomain-invoke) System.AppDomain:GetType ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.AppDomain:GetType ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: castclass <Type>
IL_00a4: ret       
// (wrapper xdomain-invoke) System.AppDomain:Load (System.Reflection.AssemblyName)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:Load (System.Reflection.AssemblyName)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a4: castclass <Byte[]>
IL_00a9: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00ae: castclass <Assembly>
IL_00b3: ret       
// (wrapper xdomain-invoke) System.AppDomain:Load (byte[])
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.AppDomain:Load (byte[])>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,byte[]>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: mono_icall <mono_marshal_xdomain_copy_value>
IL_0097: castclass <Byte[]>
IL_009c: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a1: castclass <Assembly>
IL_00a6: ret       
// (wrapper xdomain-invoke) System.AppDomain:Load (string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.AppDomain:Load (string)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,string>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: mono_icall <mono_marshal_xdomain_copy_value>
IL_0097: castclass <Byte[]>
IL_009c: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a1: castclass <Assembly>
IL_00a6: ret       
// (wrapper xdomain-invoke) System.AppDomain:Load (System.Reflection.AssemblyName,System.Security.Policy.Evidence)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003a
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: call      <(wrapper remoting-invoke) System.AppDomain:Load (System.Reflection.AssemblyName,System.Security.Policy.Evidence)>
IL_0039: ret       
IL_003a: ldc.i4.2  
IL_003b: newarr    <0x00000007>
IL_0040: dup       
IL_0041: ldc.i4.0  
IL_0042: ldarg.1   
IL_0043: stelem.ref
IL_0044: dup       
IL_0045: ldc.i4.1  
IL_0046: ldarg.2   
IL_0047: stelem.ref
IL_0048: stloc.0   
IL_0049: ldloc.0   
IL_004a: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004f: stloc.1   
IL_0050: ldloc.s   4
IL_0052: ldc.i4.1  
IL_0053: mono_icall <mono_marshal_set_domain_by_id>
IL_0059: stloc.3   
IL_005a: ldloc.2   
IL_005b: ldloca.s  1
IL_005d: ldloca.s  5
IL_005f: mono_ldptr10
IL_0065: mono_icall <mono_compile_method>
IL_006b: calli     <object,byte[]&,byte[]&>
IL_0070: ldloc.3   
IL_0071: ldc.i4.0  
IL_0072: mono_icall <mono_marshal_set_domain_by_id>
IL_0078: pop       
IL_0079: ldloc.s   6
IL_007b: mono_icall <mono_context_set>
IL_0081: ldloc.s   5
IL_0083: brfalse.s IL_00a2
IL_0085: ldloc.s   5
IL_0087: mono_icall <mono_marshal_xdomain_copy_value>
IL_008d: castclass <Byte[]>
IL_0092: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0097: castclass <Exception>
IL_009c: call      <System.Exception:FixRemotingException ()>
IL_00a1: throw     
IL_00a2: ldloc.1   
IL_00a3: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a9: castclass <Byte[]>
IL_00ae: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b3: castclass <Assembly>
IL_00b8: ret       
// (wrapper xdomain-invoke) System.AppDomain:Load (byte[],byte[])
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0038
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: ldarg.2   
IL_0032: call      <(wrapper remoting-invoke) System.AppDomain:Load (byte[],byte[])>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.3   
IL_0040: ldc.i4.1  
IL_0041: mono_icall <mono_marshal_set_domain_by_id>
IL_0047: stloc.2   
IL_0048: ldloc.1   
IL_0049: ldloca.s  0
IL_004b: ldloca.s  4
IL_004d: ldarg.1   
IL_004e: ldarg.2   
IL_004f: mono_ldptr9
IL_0055: mono_icall <mono_compile_method>
IL_005b: calli     <object,byte[]&,byte[]&,byte[],byte[]>
IL_0060: ldloc.2   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   5
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   4
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   4
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: mono_icall <mono_marshal_xdomain_copy_value>
IL_0099: castclass <Byte[]>
IL_009e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a3: castclass <Assembly>
IL_00a8: ret       
// (wrapper xdomain-invoke) System.AppDomain:Load (string,System.Security.Policy.Evidence)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003a
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: call      <(wrapper remoting-invoke) System.AppDomain:Load (string,System.Security.Policy.Evidence)>
IL_0039: ret       
IL_003a: ldc.i4.1  
IL_003b: newarr    <0x00000007>
IL_0040: dup       
IL_0041: ldc.i4.0  
IL_0042: ldarg.2   
IL_0043: stelem.ref
IL_0044: stloc.0   
IL_0045: ldloc.0   
IL_0046: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004b: stloc.1   
IL_004c: ldloc.s   4
IL_004e: ldc.i4.1  
IL_004f: mono_icall <mono_marshal_set_domain_by_id>
IL_0055: stloc.3   
IL_0056: ldloc.2   
IL_0057: ldloca.s  1
IL_0059: ldloca.s  5
IL_005b: ldarg.1   
IL_005c: mono_ldptr10
IL_0062: mono_icall <mono_compile_method>
IL_0068: calli     <object,byte[]&,byte[]&,string>
IL_006d: ldloc.3   
IL_006e: ldc.i4.0  
IL_006f: mono_icall <mono_marshal_set_domain_by_id>
IL_0075: pop       
IL_0076: ldloc.s   6
IL_0078: mono_icall <mono_context_set>
IL_007e: ldloc.s   5
IL_0080: brfalse.s IL_009f
IL_0082: ldloc.s   5
IL_0084: mono_icall <mono_marshal_xdomain_copy_value>
IL_008a: castclass <Byte[]>
IL_008f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0094: castclass <Exception>
IL_0099: call      <System.Exception:FixRemotingException ()>
IL_009e: throw     
IL_009f: ldloc.1   
IL_00a0: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a6: castclass <Byte[]>
IL_00ab: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b0: castclass <Assembly>
IL_00b5: ret       
// (wrapper xdomain-invoke) System.AppDomain:Load (byte[],byte[],System.Security.Policy.Evidence)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003b
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: call      <(wrapper remoting-invoke) System.AppDomain:Load (byte[],byte[],System.Security.Policy.Evidence)>
IL_003a: ret       
IL_003b: ldc.i4.1  
IL_003c: newarr    <0x00000007>
IL_0041: dup       
IL_0042: ldc.i4.0  
IL_0043: ldarg.3   
IL_0044: stelem.ref
IL_0045: stloc.0   
IL_0046: ldloc.0   
IL_0047: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004c: stloc.1   
IL_004d: ldloc.s   4
IL_004f: ldc.i4.1  
IL_0050: mono_icall <mono_marshal_set_domain_by_id>
IL_0056: stloc.3   
IL_0057: ldloc.2   
IL_0058: ldloca.s  1
IL_005a: ldloca.s  5
IL_005c: ldarg.1   
IL_005d: ldarg.2   
IL_005e: mono_ldptr10
IL_0064: mono_icall <mono_compile_method>
IL_006a: calli     <object,byte[]&,byte[]&,byte[],byte[]>
IL_006f: ldloc.3   
IL_0070: ldc.i4.0  
IL_0071: mono_icall <mono_marshal_set_domain_by_id>
IL_0077: pop       
IL_0078: ldloc.s   6
IL_007a: mono_icall <mono_context_set>
IL_0080: ldloc.s   5
IL_0082: brfalse.s IL_00a1
IL_0084: ldloc.s   5
IL_0086: mono_icall <mono_marshal_xdomain_copy_value>
IL_008c: castclass <Byte[]>
IL_0091: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0096: castclass <Exception>
IL_009b: call      <System.Exception:FixRemotingException ()>
IL_00a0: throw     
IL_00a1: ldloc.1   
IL_00a2: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a8: castclass <Byte[]>
IL_00ad: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b2: castclass <Assembly>
IL_00b7: ret       
// (wrapper xdomain-invoke) System.AppDomain:SetAppDomainPolicy (System.Security.Policy.PolicyLevel)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:SetAppDomainPolicy (System.Security.Policy.PolicyLevel)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:SetCachePath (string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.AppDomain:SetCachePath (string)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,string>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.AppDomain:SetData (string,object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003a
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: call      <(wrapper remoting-invoke) System.AppDomain:SetData (string,object)>
IL_0039: ret       
IL_003a: ldc.i4.1  
IL_003b: newarr    <0x00000007>
IL_0040: dup       
IL_0041: ldc.i4.0  
IL_0042: ldarg.2   
IL_0043: stelem.ref
IL_0044: stloc.0   
IL_0045: ldloc.0   
IL_0046: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004b: stloc.1   
IL_004c: ldloc.s   4
IL_004e: ldc.i4.1  
IL_004f: mono_icall <mono_marshal_set_domain_by_id>
IL_0055: stloc.3   
IL_0056: ldloc.2   
IL_0057: ldloca.s  1
IL_0059: ldloca.s  5
IL_005b: ldarg.1   
IL_005c: mono_ldptr10
IL_0062: mono_icall <mono_compile_method>
IL_0068: calli     <object,byte[]&,byte[]&,string>
IL_006d: ldloc.3   
IL_006e: ldc.i4.0  
IL_006f: mono_icall <mono_marshal_set_domain_by_id>
IL_0075: pop       
IL_0076: ldloc.s   6
IL_0078: mono_icall <mono_context_set>
IL_007e: ldloc.s   5
IL_0080: brfalse.s IL_009f
IL_0082: ldloc.s   5
IL_0084: mono_icall <mono_marshal_xdomain_copy_value>
IL_008a: castclass <Byte[]>
IL_008f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0094: castclass <Exception>
IL_0099: call      <System.Exception:FixRemotingException ()>
IL_009e: throw     
IL_009f: ldloc.1   
IL_00a0: dup       
IL_00a1: brfalse.s IL_00ae
IL_00a3: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a9: castclass <Byte[]>
IL_00ae: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b3: pop       
IL_00b4: ret       
// (wrapper xdomain-invoke) System.AppDomain:SetPrincipalPolicy (System.Security.Principal.PrincipalPolicy)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:SetPrincipalPolicy (System.Security.Principal.PrincipalPolicy)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: box       <0x00000008>
IL_0047: stelem.ref
IL_0048: stloc.0   
IL_0049: ldloc.0   
IL_004a: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004f: stloc.1   
IL_0050: ldloc.s   4
IL_0052: ldc.i4.1  
IL_0053: mono_icall <mono_marshal_set_domain_by_id>
IL_0059: stloc.3   
IL_005a: ldloc.2   
IL_005b: ldloca.s  1
IL_005d: ldloca.s  5
IL_005f: mono_ldptr11
IL_0065: mono_icall <mono_compile_method>
IL_006b: calli     <object,byte[]&,byte[]&>
IL_0070: ldloc.3   
IL_0071: ldc.i4.0  
IL_0072: mono_icall <mono_marshal_set_domain_by_id>
IL_0078: pop       
IL_0079: ldloc.s   6
IL_007b: mono_icall <mono_context_set>
IL_0081: ldloc.s   5
IL_0083: brfalse.s IL_00a2
IL_0085: ldloc.s   5
IL_0087: mono_icall <mono_marshal_xdomain_copy_value>
IL_008d: castclass <Byte[]>
IL_0092: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0097: castclass <Exception>
IL_009c: call      <System.Exception:FixRemotingException ()>
IL_00a1: throw     
IL_00a2: ldloc.1   
IL_00a3: dup       
IL_00a4: brfalse.s IL_00b1
IL_00a6: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ac: castclass <Byte[]>
IL_00b1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b6: pop       
IL_00b7: ret       
// (wrapper xdomain-invoke) System.AppDomain:SetShadowCopyPath (string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.AppDomain:SetShadowCopyPath (string)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,string>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.AppDomain:SetThreadPrincipal (System.Security.Principal.IPrincipal)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.AppDomain:SetThreadPrincipal (System.Security.Principal.IPrincipal)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.AppDomain:System._AppDomain.GetIDsOfNames (System.Guid&,intptr,uint,uint,intptr)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003f
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: ldarg.s   5
IL_0039: call      <(wrapper remoting-invoke) System.AppDomain:System._AppDomain.GetIDsOfNames (System.Guid&,intptr,uint,uint,intptr)>
IL_003e: ret       
IL_003f: ldc.i4.3  
IL_0040: newarr    <0x00000007>
IL_0045: dup       
IL_0046: ldc.i4.0  
IL_0047: ldarg.1   
IL_0048: ldobj     <0x00000008>
IL_004d: box       <0x00000009>
IL_0052: stelem.ref
IL_0053: dup       
IL_0054: ldc.i4.1  
IL_0055: ldarg.2   
IL_0056: box       <0x0000000a>
IL_005b: stelem.ref
IL_005c: dup       
IL_005d: ldc.i4.2  
IL_005e: ldarg.s   5
IL_0060: box       <0x0000000b>
IL_0065: stelem.ref
IL_0066: stloc.0   
IL_0067: ldloc.0   
IL_0068: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_006d: stloc.1   
IL_006e: ldloc.s   4
IL_0070: ldc.i4.1  
IL_0071: mono_icall <mono_marshal_set_domain_by_id>
IL_0077: stloc.3   
IL_0078: ldloc.2   
IL_0079: ldloca.s  1
IL_007b: ldloca.s  5
IL_007d: ldarg.3   
IL_007e: ldarg.s   4
IL_0080: mono_ldptr14
IL_0086: mono_icall <mono_compile_method>
IL_008c: calli     <object,byte[]&,byte[]&,uint,uint>
IL_0091: ldloc.3   
IL_0092: ldc.i4.0  
IL_0093: mono_icall <mono_marshal_set_domain_by_id>
IL_0099: pop       
IL_009a: ldloc.s   6
IL_009c: mono_icall <mono_context_set>
IL_00a2: ldloc.s   5
IL_00a4: brfalse.s IL_00c3
IL_00a6: ldloc.s   5
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <Byte[]>
IL_00b3: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b8: castclass <Exception>
IL_00bd: call      <System.Exception:FixRemotingException ()>
IL_00c2: throw     
IL_00c3: ldloc.1   
IL_00c4: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ca: castclass <Byte[]>
IL_00cf: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00d4: stloc.0   
IL_00d5: ldarg.1   
IL_00d6: ldloc.0   
IL_00d7: ldc.i4.0  
IL_00d8: ldelem.ref
IL_00d9: unbox     <0x0000001b>
IL_00de: ldobj     <0x0000001c>
IL_00e3: stobj     <0x0000001d>
IL_00e8: ret       
// (wrapper xdomain-invoke) System.AppDomain:System._AppDomain.GetTypeInfo (uint,uint,intptr)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003b
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: call      <(wrapper remoting-invoke) System.AppDomain:System._AppDomain.GetTypeInfo (uint,uint,intptr)>
IL_003a: ret       
IL_003b: ldc.i4.1  
IL_003c: newarr    <0x00000007>
IL_0041: dup       
IL_0042: ldc.i4.0  
IL_0043: ldarg.3   
IL_0044: box       <0x00000008>
IL_0049: stelem.ref
IL_004a: stloc.0   
IL_004b: ldloc.0   
IL_004c: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0051: stloc.1   
IL_0052: ldloc.s   4
IL_0054: ldc.i4.1  
IL_0055: mono_icall <mono_marshal_set_domain_by_id>
IL_005b: stloc.3   
IL_005c: ldloc.2   
IL_005d: ldloca.s  1
IL_005f: ldloca.s  5
IL_0061: ldarg.1   
IL_0062: ldarg.2   
IL_0063: mono_ldptr11
IL_0069: mono_icall <mono_compile_method>
IL_006f: calli     <object,byte[]&,byte[]&,uint,uint>
IL_0074: ldloc.3   
IL_0075: ldc.i4.0  
IL_0076: mono_icall <mono_marshal_set_domain_by_id>
IL_007c: pop       
IL_007d: ldloc.s   6
IL_007f: mono_icall <mono_context_set>
IL_0085: ldloc.s   5
IL_0087: brfalse.s IL_00a6
IL_0089: ldloc.s   5
IL_008b: mono_icall <mono_marshal_xdomain_copy_value>
IL_0091: castclass <Byte[]>
IL_0096: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009b: castclass <Exception>
IL_00a0: call      <System.Exception:FixRemotingException ()>
IL_00a5: throw     
IL_00a6: ldloc.1   
IL_00a7: dup       
IL_00a8: brfalse.s IL_00b5
IL_00aa: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b0: castclass <Byte[]>
IL_00b5: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00ba: pop       
IL_00bb: ret       
// (wrapper xdomain-invoke) System.AppDomain:System._AppDomain.GetTypeInfoCount (uint&)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.AppDomain:System._AppDomain.GetTypeInfoCount (uint&)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,uint&>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.AppDomain:System._AppDomain.Invoke (uint,System.Guid&,uint,int16,intptr,intptr,intptr,intptr)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0045
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: ldarg.s   5
IL_0039: ldarg.s   6
IL_003b: ldarg.s   7
IL_003d: ldarg.s   8
IL_003f: call      <(wrapper remoting-invoke) System.AppDomain:System._AppDomain.Invoke (uint,System.Guid&,uint,int16,intptr,intptr,intptr,intptr)>
IL_0044: ret       
IL_0045: ldc.i4.5  
IL_0046: newarr    <0x00000007>
IL_004b: dup       
IL_004c: ldc.i4.0  
IL_004d: ldarg.2   
IL_004e: ldobj     <0x00000008>
IL_0053: box       <0x00000009>
IL_0058: stelem.ref
IL_0059: dup       
IL_005a: ldc.i4.1  
IL_005b: ldarg.s   5
IL_005d: box       <0x0000000a>
IL_0062: stelem.ref
IL_0063: dup       
IL_0064: ldc.i4.2  
IL_0065: ldarg.s   6
IL_0067: box       <0x0000000b>
IL_006c: stelem.ref
IL_006d: dup       
IL_006e: ldc.i4.3  
IL_006f: ldarg.s   7
IL_0071: box       <0x0000000c>
IL_0076: stelem.ref
IL_0077: dup       
IL_0078: ldc.i4.4  
IL_0079: ldarg.s   8
IL_007b: box       <0x0000000d>
IL_0080: stelem.ref
IL_0081: stloc.0   
IL_0082: ldloc.0   
IL_0083: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0088: stloc.1   
IL_0089: ldloc.s   4
IL_008b: ldc.i4.1  
IL_008c: mono_icall <mono_marshal_set_domain_by_id>
IL_0092: stloc.3   
IL_0093: ldloc.2   
IL_0094: ldloca.s  1
IL_0096: ldloca.s  5
IL_0098: ldarg.1   
IL_0099: ldarg.3   
IL_009a: ldarg.s   4
IL_009c: mono_ldptr16
IL_00a2: mono_icall <mono_compile_method>
IL_00a8: calli     <object,byte[]&,byte[]&,uint,uint,int16>
IL_00ad: ldloc.3   
IL_00ae: ldc.i4.0  
IL_00af: mono_icall <mono_marshal_set_domain_by_id>
IL_00b5: pop       
IL_00b6: ldloc.s   6
IL_00b8: mono_icall <mono_context_set>
IL_00be: ldloc.s   5
IL_00c0: brfalse.s IL_00df
IL_00c2: ldloc.s   5
IL_00c4: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ca: castclass <Byte[]>
IL_00cf: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00d4: castclass <Exception>
IL_00d9: call      <System.Exception:FixRemotingException ()>
IL_00de: throw     
IL_00df: ldloc.1   
IL_00e0: mono_icall <mono_marshal_xdomain_copy_value>
IL_00e6: castclass <Byte[]>
IL_00eb: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00f0: stloc.0   
IL_00f1: ldarg.2   
IL_00f2: ldloc.0   
IL_00f3: ldc.i4.0  
IL_00f4: ldelem.ref
IL_00f5: unbox     <0x0000001d>
IL_00fa: ldobj     <0x0000001e>
IL_00ff: stobj     <0x0000001f>
IL_0104: ret       
// (wrapper managed-to-native) System.Runtime.InteropServices.GCHandle:GetTargetHandle (object,int,System.Runtime.InteropServices.GCHandleType)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <object,int,GCHandleType>
IL_000e: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:get_Evidence ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.AppDomain:get_Evidence ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.AppDomain:get_Evidence ()>
IL_0031: ret       
IL_0032: call      <System.AppDomain:get_Evidence ()>
IL_0037: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:GetEntryAssembly ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:get_code_base (bool)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <bool>
IL_0019: ret       
// (wrapper managed-to-native) System.Security.Policy.Evidence:IsAuthenticodePresent (System.Reflection.Assembly)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Assembly>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:get_global_assembly_cache ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:get_DomainManager ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.AppDomain:get_DomainManager ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.AppDomain:get_DomainManager ()>
IL_0031: ret       
IL_0032: call      <System.AppDomain:get_DomainManager ()>
IL_0037: ret       
// (wrapper stfld-remote) object:__mono_store_remote_field_new_wrapper (object,intptr,intptr,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: mono_ldptr2
IL_000a: calli     <object,intptr,intptr,object>
IL_000f: mono_ldptr4
IL_0015: ldind.u4  
IL_0016: brfalse   IL_0023
IL_001b: mono_not_taken
IL_001d: mono_icall <mono_thread_interruption_checkpoint>
IL_0023: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_store_remote_field_new (object,intptr,intptr,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: mono_ldptr2
IL_000a: calli     <object,intptr,intptr,object>
IL_000f: ret       
// (wrapper runtime-invoke) object:runtime_invoke_object_object_object_intptr_intptr (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.1   
IL_001b: ldind.ref 
IL_001c: ldarg.1   
IL_001d: ldc.i4.s  8
IL_001f: add       
IL_0020: ldind.ref 
IL_0021: ldarg.1   
IL_0022: ldc.i4.s  16
IL_0024: add       
IL_0025: ldind.i   
IL_0026: ldarg.1   
IL_0027: ldc.i4.s  24
IL_0029: add       
IL_002a: ldind.i   
IL_002b: ldarg.3   
IL_002c: calli     <object,object,intptr,intptr>
IL_0031: stloc.0   
IL_0032: leave     IL_0050
IL_0037: pop       
IL_0038: ldarg.2   
IL_0039: ldc.i4.0  
IL_003a: cgt.un    
IL_003c: endfilter 
IL_003e: stloc.1   
IL_003f: ldarg.2   
IL_0040: ldloc.1   
IL_0041: stind.ref 
IL_0042: ldnull    
IL_0043: stloc.0   
IL_0044: ldloc.1   
IL_0045: mono_icall <runtime_invoke_reset_abort>
IL_004b: leave     IL_0050
IL_0050: ldloc.0   
IL_0051: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:DefineInternalDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_0034
IL_0015: ldarg.0   
IL_0016: mono_objaddr
IL_0018: ldc.i4.s  16
IL_001a: add       
IL_001b: ldind.ref 
IL_001c: mono_objaddr
IL_001e: ldc.i4.s  40
IL_0020: add       
IL_0021: ldind.i4  
IL_0022: ldc.i4.m1 
IL_0023: beq       IL_002e
IL_0028: call      <(wrapper xdomain-invoke) System.AppDomain:DefineInternalDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)>
IL_002d: ret       
IL_002e: call      <(wrapper remoting-invoke) System.AppDomain:DefineInternalDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)>
IL_0033: ret       
IL_0034: call      <System.AppDomain:DefineInternalDynamicAssembly (System.Reflection.AssemblyName,System.Reflection.Emit.AssemblyBuilderAccess)>
IL_0039: ret       
// (wrapper managed-to-native) System.Reflection.Emit.AssemblyBuilder:basic_init (System.Reflection.Emit.AssemblyBuilder)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <AssemblyBuilder>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:basic_init (System.Reflection.Emit.ModuleBuilder)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <ModuleBuilder>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.Emit.TypeBuilder:setup_internal_class (System.Reflection.Emit.TypeBuilder)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <TypeBuilder>
IL_0019: ret       
// (wrapper managed-to-native) System.Reflection.Emit.TypeBuilder:create_generic_class ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Reflection.Emit.TypeBuilder:create_runtime_class (System.Reflection.Emit.TypeBuilder)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <TypeBuilder>
IL_0019: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:set_wrappers_type (System.Reflection.Emit.ModuleBuilder,System.Type)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <ModuleBuilder,Type>
IL_000d: ret       
// (wrapper managed-to-native) System.MonoType:GetFields_internal (System.Reflection.BindingFlags,System.Type)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <BindingFlags,Type>
IL_001a: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:RegisterToken (object,int)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <object,int>
IL_001a: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:getToken (System.Reflection.Emit.ModuleBuilder,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <ModuleBuilder,object>
IL_000d: ret       
// (wrapper managed-to-native) System.Reflection.Emit.ModuleBuilder:getUSIndex (System.Reflection.Emit.ModuleBuilder,string)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <ModuleBuilder,string>
IL_000d: ret       
// Evidence__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000001>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// Evidence__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// Evidence__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    4
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.2   
IL_0017: ldstr     
IL_001c: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0021: ldarg.2   
IL_0022: ldstr     
IL_0027: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002c: ldarg.2   
IL_002d: ldstr     
IL_0032: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0037: ldarg.3   
IL_0038: brfalse   IL_0083
IL_003d: ldarg.2   
IL_003e: ldc.i4.s  0
IL_0040: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0045: ldarg.2   
IL_0046: ldc.i4.s  3
IL_0048: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_004d: ldarg.2   
IL_004e: ldc.i4.s  3
IL_0050: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0055: ldarg.2   
IL_0056: ldc.i4.s  0
IL_0058: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_005d: ldarg.2   
IL_005e: ldc.i4.s  1
IL_0060: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0065: ldarg.2   
IL_0066: ldstr     
IL_006b: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0070: ldarg.2   
IL_0071: ldstr     
IL_0076: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_007b: ldarg.2   
IL_007c: ldc.i4.s  8
IL_007e: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0083: ret       
// Evidence__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <Evidence>
IL_0006: stloc.0   
IL_0007: ldarg.2   
IL_0008: ldloc.0   
IL_0009: ldfld     <System.Security.Policy.Evidence:_locked>
IL_000e: callvirt  <System.IO.BinaryWriter:Write (bool)>
IL_0013: ldarg.1   
IL_0014: ldarg.2   
IL_0015: ldtoken   <0x01000005>
IL_001a: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_001f: ldloc.0   
IL_0020: ldfld     <System.Security.Policy.Evidence:hostEvidenceList>
IL_0025: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_002a: ldarg.1   
IL_002b: ldarg.2   
IL_002c: ldtoken   <0x01000005>
IL_0031: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0036: ldloc.0   
IL_0037: ldfld     <System.Security.Policy.Evidence:assemblyEvidenceList>
IL_003c: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0041: ldarg.2   
IL_0042: ldloc.0   
IL_0043: ldfld     <System.Security.Policy.Evidence:_hashCode>
IL_0048: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_004d: ret       
// (wrapper managed-to-native) System.ValueType:InternalEquals (object,object,object[]&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <object,object,object[]&>
IL_000e: ret       
// SynchronizedArrayListWrapper__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000007>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// SynchronizedArrayListWrapper__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// SynchronizedArrayListWrapper__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    5
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.2   
IL_0017: ldstr     
IL_001c: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0021: ldarg.2   
IL_0022: ldstr     
IL_0027: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002c: ldarg.2   
IL_002d: ldstr     
IL_0032: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0037: ldarg.2   
IL_0038: ldstr     
IL_003d: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0042: ldarg.3   
IL_0043: brfalse   IL_008b
IL_0048: ldarg.2   
IL_0049: ldc.i4.s  2
IL_004b: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0050: ldarg.2   
IL_0051: ldc.i4.s  3
IL_0053: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0058: ldarg.2   
IL_0059: ldc.i4.s  0
IL_005b: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0060: ldarg.2   
IL_0061: ldc.i4.s  5
IL_0063: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0068: ldarg.2   
IL_0069: ldc.i4.s  0
IL_006b: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0070: ldarg.2   
IL_0071: ldstr     
IL_0076: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_007b: ldarg.2   
IL_007c: ldc.i4.s  8
IL_007e: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0083: ldarg.2   
IL_0084: ldc.i4.s  8
IL_0086: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_008b: ret       
// SynchronizedArrayListWrapper__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <SynchronizedArrayListWrapper>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x01000008>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <SynchronizedArrayListWrapper:m_SyncRoot>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ldarg.1   
IL_001f: ldarg.2   
IL_0020: ldtoken   <0x01000005>
IL_0025: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_002a: ldloc.0   
IL_002b: ldfld     <ArrayListWrapper:m_InnerArrayList>
IL_0030: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0035: ldarg.2   
IL_0036: ldloc.0   
IL_0037: ldfld     <System.Collections.ArrayList:_size>
IL_003c: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_0041: ldarg.1   
IL_0042: ldarg.2   
IL_0043: ldtoken   <0x1b000001>
IL_0048: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_004d: ldloc.0   
IL_004e: ldfld     <System.Collections.ArrayList:_items>
IL_0053: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0058: ldarg.2   
IL_0059: ldloc.0   
IL_005a: ldfld     <System.Collections.ArrayList:_version>
IL_005f: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_0064: ret       
// ArrayList__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000005>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// ArrayList__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// ArrayList__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    3
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.2   
IL_0017: ldstr     
IL_001c: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0021: ldarg.2   
IL_0022: ldstr     
IL_0027: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002c: ldarg.3   
IL_002d: brfalse   IL_005a
IL_0032: ldarg.2   
IL_0033: ldc.i4.s  0
IL_0035: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_003a: ldarg.2   
IL_003b: ldc.i4.s  5
IL_003d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0042: ldarg.2   
IL_0043: ldc.i4.s  0
IL_0045: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_004a: ldarg.2   
IL_004b: ldc.i4.s  8
IL_004d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0052: ldarg.2   
IL_0053: ldc.i4.s  8
IL_0055: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_005a: ret       
// ArrayList__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <ArrayList>
IL_0006: stloc.0   
IL_0007: ldarg.2   
IL_0008: ldloc.0   
IL_0009: ldfld     <System.Collections.ArrayList:_size>
IL_000e: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_0013: ldarg.1   
IL_0014: ldarg.2   
IL_0015: ldtoken   <0x1b000001>
IL_001a: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_001f: ldloc.0   
IL_0020: ldfld     <System.Collections.ArrayList:_items>
IL_0025: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_002a: ldarg.2   
IL_002b: ldloc.0   
IL_002c: ldfld     <System.Collections.ArrayList:_version>
IL_0031: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_0036: ret       
// Zone__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x0100000a>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// Zone__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// Zone__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    1
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.3   
IL_0017: brfalse   IL_002f
IL_001c: ldarg.2   
IL_001d: ldc.i4.s  3
IL_001f: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0024: ldarg.2   
IL_0025: ldstr     
IL_002a: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002f: ret       
// Zone__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <Zone>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x0100000b>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Security.Policy.Zone:zone>
IL_0019: box       <0x0100000b>
IL_001e: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0023: ret       
// SecurityZone__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x0100000b>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// SecurityZone__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// SecurityZone__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    1
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.3   
IL_0017: brfalse   IL_002c
IL_001c: ldarg.2   
IL_001d: ldc.i4.s  0
IL_001f: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0024: ldarg.2   
IL_0025: ldc.i4.s  8
IL_0027: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_002c: ret       
// SecurityZone__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: unbox     <0x0100000b>
IL_0006: ldind.i4  
IL_0007: stloc.0   
IL_0008: ldarg.2   
IL_0009: ldloca.s  0
IL_000b: ldfld     <System.Security.SecurityZone:value__>
IL_0010: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_0015: ret       
// Url__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x0100000c>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// Url__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// Url__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    1
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.3   
IL_0017: brfalse   IL_0024
IL_001c: ldarg.2   
IL_001d: ldc.i4.s  1
IL_001f: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0024: ret       
// Url__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <Url>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x0100000d>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Security.Policy.Url:origin_url>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:get_location ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper remoting-invoke-with-check) System.IO.FileStream:.ctor (string,System.IO.FileMode,System.IO.FileAccess)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.0   
IL_0005: mono_objaddr
IL_0007: ldind.i   
IL_0008: ldc.i4.0  
IL_0009: add       
IL_000a: ldind.i   
IL_000b: mono_classconst <TransparentProxy>
IL_0011: bne.un    IL_001c
IL_0016: call      <(wrapper remoting-invoke) System.IO.FileStream:.ctor (string,System.IO.FileMode,System.IO.FileAccess)>
IL_001b: ret       
IL_001c: call      <System.IO.FileStream:.ctor (string,System.IO.FileMode,System.IO.FileAccess)>
IL_0021: ret       
// (wrapper managed-to-native) System.AppDomain:InternalPushDomainRefByID (int)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <int>
IL_000c: ret       
// (wrapper managed-to-native) System.AppDomain:InternalSetDomainByID (int)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <int>
IL_000c: ret       
// (wrapper runtime-invoke) object:runtime_invoke_CrossAppDomainSink/ProcessMessageRes_object_object (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.1   
IL_001b: ldind.ref 
IL_001c: ldarg.1   
IL_001d: ldc.i4.s  8
IL_001f: add       
IL_0020: ldind.ref 
IL_0021: ldarg.3   
IL_0022: calli     <object,object>
IL_0027: box       <0x00000005>
IL_002c: stloc.0   
IL_002d: leave     IL_004b
IL_0032: pop       
IL_0033: ldarg.2   
IL_0034: ldc.i4.0  
IL_0035: cgt.un    
IL_0037: endfilter 
IL_0039: stloc.1   
IL_003a: ldarg.2   
IL_003b: ldloc.1   
IL_003c: stind.ref 
IL_003d: ldnull    
IL_003e: stloc.0   
IL_003f: ldloc.1   
IL_0040: mono_icall <runtime_invoke_reset_abort>
IL_0046: leave     IL_004b
IL_004b: ldloc.0   
IL_004c: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:ProcessMessageInDomain (byte[],System.Runtime.Remoting.Messaging.CADMethodCallMessage,byte[]&,System.Runtime.Remoting.Messaging.CADMethodReturnMessage&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.0   
IL_0007: mono_objaddr
IL_0009: ldind.i   
IL_000a: ldc.i4.0  
IL_000b: add       
IL_000c: ldind.i   
IL_000d: mono_classconst <TransparentProxy>
IL_0013: bne.un    IL_0037
IL_0018: ldarg.0   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  16
IL_001d: add       
IL_001e: ldind.ref 
IL_001f: mono_objaddr
IL_0021: ldc.i4.s  40
IL_0023: add       
IL_0024: ldind.i4  
IL_0025: ldc.i4.m1 
IL_0026: beq       IL_0031
IL_002b: call      <(wrapper xdomain-invoke) System.AppDomain:ProcessMessageInDomain (byte[],System.Runtime.Remoting.Messaging.CADMethodCallMessage,byte[]&,System.Runtime.Remoting.Messaging.CADMethodReturnMessage&)>
IL_0030: ret       
IL_0031: call      <(wrapper remoting-invoke) System.AppDomain:ProcessMessageInDomain (byte[],System.Runtime.Remoting.Messaging.CADMethodCallMessage,byte[]&,System.Runtime.Remoting.Messaging.CADMethodReturnMessage&)>
IL_0036: ret       
IL_0037: call      <System.AppDomain:ProcessMessageInDomain (byte[],System.Runtime.Remoting.Messaging.CADMethodCallMessage,byte[]&,System.Runtime.Remoting.Messaging.CADMethodReturnMessage&)>
IL_003c: ret       
// (wrapper remoting-invoke-with-check) System.IO.MemoryStream:.ctor (byte[])
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_001a
IL_0014: call      <(wrapper remoting-invoke) System.IO.MemoryStream:.ctor (byte[])>
IL_0019: ret       
IL_001a: call      <System.IO.MemoryStream:.ctor (byte[])>
IL_001f: ret       
// (wrapper managed-to-native) System.MonoType:GetField (string,System.Reflection.BindingFlags)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <string,BindingFlags>
IL_001a: ret       
// (wrapper managed-to-native) System.Runtime.Remoting.Activation.ActivationServices:AllocateUninitializedClassInstance (System.Type)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <Type>
IL_000c: ret       
// (wrapper managed-to-native) System.Reflection.MonoField:SetValueInternal (System.Reflection.FieldInfo,object,object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <FieldInfo,object,object>
IL_000e: ret       
// (wrapper managed-to-native) System.Type:IsInstanceOfType (object)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <object>
IL_0019: ret       
// (wrapper managed-to-native) System.Array:SetValue (object,int[])
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: mono_ldptr3
IL_0015: calli     <object,int[]>
IL_001a: ret       
// (wrapper runtime-invoke) object:runtime_invoke_void__this___object_StreamingContext (object,intptr,intptr,intptr)
IL_0000: ldarg.2   
IL_0001: brfalse.s IL_0006
IL_0003: ldarg.2   
IL_0004: ldnull    
IL_0005: stind.ref 
IL_0006: mono_ldptr2
IL_000c: ldind.u4  
IL_000d: brfalse   IL_001a
IL_0012: mono_not_taken
IL_0014: mono_icall <mono_thread_force_interruption_checkpoint>
IL_001a: ldarg.0   
IL_001b: ldarg.1   
IL_001c: ldind.ref 
IL_001d: ldarg.1   
IL_001e: ldc.i4.s  8
IL_0020: add       
IL_0021: ldind.i   
IL_0022: ldobj     <0x00000004>
IL_0027: ldarg.3   
IL_0028: calli     <object,StreamingContext>
IL_002d: ldnull    
IL_002e: stloc.0   
IL_002f: leave     IL_004d
IL_0034: pop       
IL_0035: ldarg.2   
IL_0036: ldc.i4.0  
IL_0037: cgt.un    
IL_0039: endfilter 
IL_003b: stloc.1   
IL_003c: ldarg.2   
IL_003d: ldloc.1   
IL_003e: stind.ref 
IL_003f: ldnull    
IL_0040: stloc.0   
IL_0041: ldloc.1   
IL_0042: mono_icall <runtime_invoke_reset_abort>
IL_0048: leave     IL_004d
IL_004d: ldloc.0   
IL_004e: ret       
// (wrapper managed-to-native) System.Reflection.MethodBase:GetMethodFromHandleInternalType (intptr,intptr)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <intptr,intptr>
IL_000d: ret       
// (wrapper managed-to-native) System.AppDomain:InternalGetContext ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Reflection.MonoMethod:get_IsGenericMethod ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Runtime.Remoting.RemotingServices:InternalExecute (System.Reflection.MethodBase,object,object[],object[]&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: mono_ldptr2
IL_000a: calli     <MethodBase,object,object[],object[]&>
IL_000f: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:CreateInstance (string,string)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_0034
IL_0015: ldarg.0   
IL_0016: mono_objaddr
IL_0018: ldc.i4.s  16
IL_001a: add       
IL_001b: ldind.ref 
IL_001c: mono_objaddr
IL_001e: ldc.i4.s  40
IL_0020: add       
IL_0021: ldind.i4  
IL_0022: ldc.i4.m1 
IL_0023: beq       IL_002e
IL_0028: call      <(wrapper xdomain-invoke) System.AppDomain:CreateInstance (string,string)>
IL_002d: ret       
IL_002e: call      <(wrapper remoting-invoke) System.AppDomain:CreateInstance (string,string)>
IL_0033: ret       
IL_0034: call      <System.AppDomain:CreateInstance (string,string)>
IL_0039: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_context_get ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_marshal_check_domain_image (int,intptr)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <int,intptr>
IL_000d: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_marshal_set_domain_by_id (int,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <int,int>
IL_000d: ret       
// (wrapper xdomain-dispatch) System.AppDomain:CreateInstance (object,byte[]&,byte[]&,string,string)
IL_0000: ldnull    
IL_0001: call      <System.Runtime.Remoting.Messaging.CallContext:SetCurrentCallContext (System.Runtime.Remoting.Messaging.LogicalCallContext)>
IL_0006: pop       
IL_0007: ldarg.1   
IL_0008: ldind.ref 
IL_0009: dup       
IL_000a: brfalse.s IL_001d
IL_000c: mono_icall <mono_marshal_xdomain_copy_value>
IL_0012: castclass <Byte[]>
IL_0017: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_001c: pop       
IL_001d: ldarg.0   
IL_001e: call      <System.Runtime.Remoting.Proxies.RealProxy:GetAppDomainTarget ()>
IL_0023: ldarg.3   
IL_0024: mono_icall <mono_marshal_xdomain_copy_value>
IL_002a: castclass <String>
IL_002f: ldarg.s   4
IL_0031: mono_icall <mono_marshal_xdomain_copy_value>
IL_0037: castclass <String>
IL_003c: mono_ldptr11
IL_0042: ldind.u4  
IL_0043: brfalse   IL_0050
IL_0048: mono_not_taken
IL_004a: mono_icall <mono_thread_force_interruption_checkpoint>
IL_0050: callvirt  <System.AppDomain:CreateInstance (string,string)>
IL_0055: stloc.1   
IL_0056: ldarg.1   
IL_0057: ldloc.1   
IL_0058: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_005d: stind.ref 
IL_005e: ldarg.2   
IL_005f: ldnull    
IL_0060: stind.ref 
IL_0061: leave     IL_0074
IL_0066: call      <System.Runtime.Remoting.RemotingServices:SerializeExceptionData (System.Exception)>
IL_006b: stloc.0   
IL_006c: ldarg.2   
IL_006d: ldloc.0   
IL_006e: stind.ref 
IL_006f: leave     IL_0074
IL_0074: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:CreateInstance (string,string)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_0034
IL_0015: ldarg.0   
IL_0016: mono_objaddr
IL_0018: ldc.i4.s  16
IL_001a: add       
IL_001b: ldind.ref 
IL_001c: mono_objaddr
IL_001e: ldc.i4.s  40
IL_0020: add       
IL_0021: ldind.i4  
IL_0022: ldc.i4.m1 
IL_0023: beq       IL_002e
IL_0028: call      <(wrapper xdomain-invoke) System.AppDomain:CreateInstance (string,string)>
IL_002d: ret       
IL_002e: call      <(wrapper remoting-invoke) System.AppDomain:CreateInstance (string,string)>
IL_0033: ret       
IL_0034: call      <System.AppDomain:CreateInstance (string,string)>
IL_0039: ret       
// (wrapper remoting-invoke-with-check) System.AppDomain:Load (string,System.Security.Policy.Evidence)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_0034
IL_0015: ldarg.0   
IL_0016: mono_objaddr
IL_0018: ldc.i4.s  16
IL_001a: add       
IL_001b: ldind.ref 
IL_001c: mono_objaddr
IL_001e: ldc.i4.s  40
IL_0020: add       
IL_0021: ldind.i4  
IL_0022: ldc.i4.m1 
IL_0023: beq       IL_002e
IL_0028: call      <(wrapper xdomain-invoke) System.AppDomain:Load (string,System.Security.Policy.Evidence)>
IL_002d: ret       
IL_002e: call      <(wrapper remoting-invoke) System.AppDomain:Load (string,System.Security.Policy.Evidence)>
IL_0033: ret       
IL_0034: call      <System.AppDomain:Load (string,System.Security.Policy.Evidence)>
IL_0039: ret       
// (wrapper managed-to-native) System.AppDomain:LoadAssembly (string,System.Security.Policy.Evidence,bool)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: mono_ldptr3
IL_0016: calli     <string,Evidence,bool>
IL_001b: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:InternalGetType (System.Reflection.Module,string,bool,bool)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: ldarg.2   
IL_000f: ldarg.3   
IL_0010: ldarg.s   4
IL_0012: mono_ldptr3
IL_0018: calli     <Module,string,bool,bool>
IL_001d: ret       
// (wrapper remoting-invoke-with-check) System.Runtime.Remoting.ObjectHandle:.ctor (object)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_001a
IL_0014: call      <(wrapper remoting-invoke) System.Runtime.Remoting.ObjectHandle:.ctor (object)>
IL_0019: ret       
IL_001a: call      <System.Runtime.Remoting.ObjectHandle:.ctor (object)>
IL_001f: ret       
// (wrapper remoting-invoke-with-check) System.Runtime.Remoting.Lifetime.Lease:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.Runtime.Remoting.Lifetime.Lease:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.Runtime.Remoting.Lifetime.Lease:.ctor ()>
IL_001e: ret       
// (wrapper managed-to-native) System.Math:Round (double)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <double>
IL_000c: ret       
// (wrapper managed-to-native) System.DateTime:GetNow ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.CurrentSystemTimeZone:GetTimeZoneData (int,long[]&,string[]&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <int,long[]&,string[]&>
IL_000e: ret       
// (wrapper remoting-invoke-with-check) System.Runtime.Remoting.Lifetime.Lease:Activate ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.Runtime.Remoting.Lifetime.Lease:Activate ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.Runtime.Remoting.Lifetime.Lease:Activate ()>
IL_0031: ret       
IL_0032: call      <System.Runtime.Remoting.Lifetime.Lease:Activate ()>
IL_0037: ret       
// (wrapper remoting-invoke-with-check) System.Threading.Timer:.ctor (System.Threading.TimerCallback,object,System.TimeSpan,System.TimeSpan)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.0   
IL_0007: mono_objaddr
IL_0009: ldind.i   
IL_000a: ldc.i4.0  
IL_000b: add       
IL_000c: ldind.i   
IL_000d: mono_classconst <TransparentProxy>
IL_0013: bne.un    IL_001e
IL_0018: call      <(wrapper remoting-invoke) System.Threading.Timer:.ctor (System.Threading.TimerCallback,object,System.TimeSpan,System.TimeSpan)>
IL_001d: ret       
IL_001e: call      <System.Threading.Timer:.ctor (System.Threading.TimerCallback,object,System.TimeSpan,System.TimeSpan)>
IL_0023: ret       
// (wrapper managed-to-native) System.Threading.Thread:Thread_init ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Threading.Thread:SetState (System.Threading.ThreadState)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <ThreadState>
IL_0019: ret       
// (wrapper managed-to-native) System.Threading.Thread:Thread_internal (System.MulticastDelegate)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <MulticastDelegate>
IL_0019: ret       
// (wrapper managed-to-native) System.DateTime:GetTimeMonotonic ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Threading.Thread:SetName_internal (string)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <string>
IL_0019: ret       
// (wrapper managed-to-native) System.Threading.Monitor:Monitor_test_synchronised (object)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <object>
IL_000c: ret       
// (wrapper managed-to-native) System.Threading.Monitor:Monitor_pulse (object)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <object>
IL_000c: ret       
// (wrapper managed-to-native) System.Threading.Monitor:Monitor_wait (object,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <object,int>
IL_000d: ret       
// TypeInfo__TypeMetadata1:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000001>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// TypeInfo__TypeMetadata1:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// TypeInfo__TypeMetadata1:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    3
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.2   
IL_0017: ldstr     
IL_001c: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0021: ldarg.2   
IL_0022: ldstr     
IL_0027: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002c: ldarg.3   
IL_002d: brfalse   IL_004a
IL_0032: ldarg.2   
IL_0033: ldc.i4.s  1
IL_0035: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_003a: ldarg.2   
IL_003b: ldc.i4.s  6
IL_003d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0042: ldarg.2   
IL_0043: ldc.i4.s  6
IL_0045: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_004a: ret       
// TypeInfo__TypeMetadata1:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <TypeInfo>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x01000005>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Runtime.Remoting.TypeInfo:serverType>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ldarg.1   
IL_001f: ldarg.2   
IL_0020: ldtoken   <0x1b000001>
IL_0025: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_002a: ldloc.0   
IL_002b: ldfld     <System.Runtime.Remoting.TypeInfo:serverHierarchy>
IL_0030: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0035: ldarg.1   
IL_0036: ldarg.2   
IL_0037: ldtoken   <0x1b000001>
IL_003c: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0041: ldloc.0   
IL_0042: ldfld     <System.Runtime.Remoting.TypeInfo:interfacesImplemented>
IL_0047: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_004c: ret       
// ChannelInfo__TypeMetadata1:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000007>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// ChannelInfo__TypeMetadata1:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// ChannelInfo__TypeMetadata1:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    1
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.3   
IL_0017: brfalse   IL_0024
IL_001c: ldarg.2   
IL_001d: ldc.i4.s  5
IL_001f: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0024: ret       
// ChannelInfo__TypeMetadata1:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <ChannelInfo>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x1b000002>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Runtime.Remoting.ChannelInfo:channelData>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ret       
// CrossAppDomainData__TypeMetadata1:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000008>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// CrossAppDomainData__TypeMetadata1:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// CrossAppDomainData__TypeMetadata1:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    3
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.2   
IL_0017: ldstr     
IL_001c: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0021: ldarg.2   
IL_0022: ldstr     
IL_0027: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002c: ldarg.3   
IL_002d: brfalse   IL_0052
IL_0032: ldarg.2   
IL_0033: ldc.i4.s  2
IL_0035: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_003a: ldarg.2   
IL_003b: ldc.i4.s  0
IL_003d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0042: ldarg.2   
IL_0043: ldc.i4.s  1
IL_0045: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_004a: ldarg.2   
IL_004b: ldc.i4.s  8
IL_004d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0052: ret       
// CrossAppDomainData__TypeMetadata1:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <CrossAppDomainData>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x01000009>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Runtime.Remoting.Channels.CrossAppDomainData:_ContextID>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ldarg.2   
IL_001f: ldloc.0   
IL_0020: ldfld     <System.Runtime.Remoting.Channels.CrossAppDomainData:_DomainID>
IL_0025: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_002a: ldarg.1   
IL_002b: ldarg.2   
IL_002c: ldtoken   <0x01000005>
IL_0031: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0036: ldloc.0   
IL_0037: ldfld     <System.Runtime.Remoting.Channels.CrossAppDomainData:_processGuid>
IL_003c: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0041: ret       
// (wrapper managed-to-native) object:__icall_wrapper_mono_context_set (object)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <object>
IL_000c: ret       
// (wrapper xdomain-invoke) object:ToString ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) object:ToString ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <String>
IL_00b3: ret       
// (wrapper xdomain-invoke) System.Runtime.Remoting.ObjectHandle:InitializeLifetimeService ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.Runtime.Remoting.ObjectHandle:InitializeLifetimeService ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: ret       
// (wrapper xdomain-invoke) System.Runtime.Remoting.ObjectHandle:Unwrap ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.Runtime.Remoting.ObjectHandle:Unwrap ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: ret       
// (wrapper proxy_isinst) object:__proxy_isinst_wrapper_IObjectReference (object)
IL_0000: ldarg.0   
IL_0001: mono_objaddr
IL_0003: ldc.i4.s  16
IL_0005: add       
IL_0006: ldind.ref 
IL_0007: mono_ldptr2
IL_000d: mono_icall <type_from_handle>
IL_0013: ldarg.0   
IL_0014: callvirt  <System.Runtime.Remoting.IRemotingTypeInfo:CanCastTo (System.Type,object)>
IL_0019: brfalse   IL_004b
IL_001e: mono_ldptr5
IL_0024: mono_icall <type_from_handle>
IL_002a: ldarg.0   
IL_002b: mono_icall <mono_upgrade_remote_class_wrapper>
IL_0031: mono_ldptr8
IL_0037: ldind.u4  
IL_0038: brfalse   IL_0045
IL_003d: mono_not_taken
IL_003f: mono_icall <mono_thread_interruption_checkpoint>
IL_0045: ldarg.0   
IL_0046: br        IL_004c
IL_004b: ldnull    
IL_004c: ret       
// (wrapper managed-to-native) object:__icall_wrapper_type_from_handle (intptr)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <intptr>
IL_000c: ret       
// (wrapper remoting-invoke-with-check) System.Runtime.Remoting.ObjectHandle:Unwrap ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.Runtime.Remoting.ObjectHandle:Unwrap ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.Runtime.Remoting.ObjectHandle:Unwrap ()>
IL_0031: ret       
IL_0032: call      <System.Runtime.Remoting.ObjectHandle:Unwrap ()>
IL_0037: ret       
// (wrapper xdomain-dispatch) System.Runtime.Remoting.ObjectHandle:Unwrap (object,byte[]&,byte[]&)
IL_0000: ldnull    
IL_0001: call      <System.Runtime.Remoting.Messaging.CallContext:SetCurrentCallContext (System.Runtime.Remoting.Messaging.LogicalCallContext)>
IL_0006: pop       
IL_0007: ldarg.1   
IL_0008: ldind.ref 
IL_0009: dup       
IL_000a: brfalse.s IL_001d
IL_000c: mono_icall <mono_marshal_xdomain_copy_value>
IL_0012: castclass <Byte[]>
IL_0017: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_001c: pop       
IL_001d: ldarg.0   
IL_001e: call      <System.Runtime.Remoting.Proxies.RealProxy:GetAppDomainTarget ()>
IL_0023: mono_ldptr7
IL_0029: ldind.u4  
IL_002a: brfalse   IL_0037
IL_002f: mono_not_taken
IL_0031: mono_icall <mono_thread_force_interruption_checkpoint>
IL_0037: callvirt  <System.Runtime.Remoting.ObjectHandle:Unwrap ()>
IL_003c: stloc.1   
IL_003d: ldarg.1   
IL_003e: ldloc.1   
IL_003f: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0044: stind.ref 
IL_0045: ldarg.2   
IL_0046: ldnull    
IL_0047: stind.ref 
IL_0048: leave     IL_005b
IL_004d: call      <System.Runtime.Remoting.RemotingServices:SerializeExceptionData (System.Exception)>
IL_0052: stloc.0   
IL_0053: ldarg.2   
IL_0054: ldloc.0   
IL_0055: stind.ref 
IL_0056: leave     IL_005b
IL_005b: ret       
// (wrapper remoting-invoke-with-check) System.Runtime.Remoting.ObjectHandle:Unwrap ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.Runtime.Remoting.ObjectHandle:Unwrap ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.Runtime.Remoting.ObjectHandle:Unwrap ()>
IL_0031: ret       
IL_0032: call      <System.Runtime.Remoting.ObjectHandle:Unwrap ()>
IL_0037: ret       
// (wrapper xdomain-invoke) System.MarshalByRefObject:InitializeLifetimeService ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.MarshalByRefObject:InitializeLifetimeService ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: ret       
// (wrapper remoting-invoke-with-check) Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.0   
IL_0007: mono_objaddr
IL_0009: ldind.i   
IL_000a: ldc.i4.0  
IL_000b: add       
IL_000c: ldind.i   
IL_000d: mono_classconst <TransparentProxy>
IL_0013: bne.un    IL_0037
IL_0018: ldarg.0   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  16
IL_001d: add       
IL_001e: ldind.ref 
IL_001f: mono_objaddr
IL_0021: ldc.i4.s  40
IL_0023: add       
IL_0024: ldind.i4  
IL_0025: ldc.i4.m1 
IL_0026: beq       IL_0031
IL_002b: call      <(wrapper xdomain-invoke) Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_0030: ret       
IL_0031: call      <(wrapper remoting-invoke) Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_0036: ret       
IL_0037: call      <Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_003c: ret       
// (wrapper xdomain-invoke) Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003d
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: call      <(wrapper remoting-invoke) Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_003c: ret       
IL_003d: ldc.i4.2  
IL_003e: newarr    <0x00000007>
IL_0043: dup       
IL_0044: ldc.i4.0  
IL_0045: ldarg.3   
IL_0046: stelem.ref
IL_0047: dup       
IL_0048: ldc.i4.1  
IL_0049: ldarg.s   4
IL_004b: stelem.ref
IL_004c: stloc.0   
IL_004d: ldloc.0   
IL_004e: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0053: stloc.1   
IL_0054: ldloc.s   4
IL_0056: ldc.i4.1  
IL_0057: mono_icall <mono_marshal_set_domain_by_id>
IL_005d: stloc.3   
IL_005e: ldloc.2   
IL_005f: ldloca.s  1
IL_0061: ldloca.s  5
IL_0063: ldarg.1   
IL_0064: ldarg.2   
IL_0065: mono_ldptr10
IL_006b: mono_icall <mono_compile_method>
IL_0071: calli     <object,byte[]&,byte[]&,string,string>
IL_0076: ldloc.3   
IL_0077: ldc.i4.0  
IL_0078: mono_icall <mono_marshal_set_domain_by_id>
IL_007e: pop       
IL_007f: ldloc.s   6
IL_0081: mono_icall <mono_context_set>
IL_0087: ldloc.s   5
IL_0089: brfalse.s IL_00a8
IL_008b: ldloc.s   5
IL_008d: mono_icall <mono_marshal_xdomain_copy_value>
IL_0093: castclass <Byte[]>
IL_0098: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009d: castclass <Exception>
IL_00a2: call      <System.Exception:FixRemotingException ()>
IL_00a7: throw     
IL_00a8: ldloc.1   
IL_00a9: dup       
IL_00aa: brfalse.s IL_00b7
IL_00ac: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b2: castclass <Byte[]>
IL_00b7: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00bc: pop       
IL_00bd: ret       
// (wrapper managed-to-native) System.Runtime.Remoting.RemotingServices:IsTransparentProxy (object)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <object>
IL_000c: ret       
// (wrapper remoting-invoke-with-check) System.MarshalByRefObject:get_ObjectIdentity ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.MarshalByRefObject:get_ObjectIdentity ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.MarshalByRefObject:get_ObjectIdentity ()>
IL_0031: ret       
IL_0032: call      <System.MarshalByRefObject:get_ObjectIdentity ()>
IL_0037: ret       
// (wrapper managed-to-native) System.Environment:get_TickCount ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper remoting-invoke-with-check) System.MarshalByRefObject:set_ObjectIdentity (System.Runtime.Remoting.ServerIdentity)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.0   
IL_0003: mono_objaddr
IL_0005: ldind.i   
IL_0006: ldc.i4.0  
IL_0007: add       
IL_0008: ldind.i   
IL_0009: mono_classconst <TransparentProxy>
IL_000f: bne.un    IL_0033
IL_0014: ldarg.0   
IL_0015: mono_objaddr
IL_0017: ldc.i4.s  16
IL_0019: add       
IL_001a: ldind.ref 
IL_001b: mono_objaddr
IL_001d: ldc.i4.s  40
IL_001f: add       
IL_0020: ldind.i4  
IL_0021: ldc.i4.m1 
IL_0022: beq       IL_002d
IL_0027: call      <(wrapper xdomain-invoke) System.MarshalByRefObject:set_ObjectIdentity (System.Runtime.Remoting.ServerIdentity)>
IL_002c: ret       
IL_002d: call      <(wrapper remoting-invoke) System.MarshalByRefObject:set_ObjectIdentity (System.Runtime.Remoting.ServerIdentity)>
IL_0032: ret       
IL_0033: call      <System.MarshalByRefObject:set_ObjectIdentity (System.Runtime.Remoting.ServerIdentity)>
IL_0038: ret       
// (wrapper remoting-invoke-with-check) System.Runtime.Remoting.Lifetime.Lease:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.Runtime.Remoting.Lifetime.Lease:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.Runtime.Remoting.Lifetime.Lease:.ctor ()>
IL_001e: ret       
// (wrapper managed-to-native) System.Math:Round (double)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <double>
IL_000c: ret       
// (wrapper remoting-invoke-with-check) System.Runtime.Remoting.Lifetime.Lease:Activate ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0032
IL_0013: ldarg.0   
IL_0014: mono_objaddr
IL_0016: ldc.i4.s  16
IL_0018: add       
IL_0019: ldind.ref 
IL_001a: mono_objaddr
IL_001c: ldc.i4.s  40
IL_001e: add       
IL_001f: ldind.i4  
IL_0020: ldc.i4.m1 
IL_0021: beq       IL_002c
IL_0026: call      <(wrapper xdomain-invoke) System.Runtime.Remoting.Lifetime.Lease:Activate ()>
IL_002b: ret       
IL_002c: call      <(wrapper remoting-invoke) System.Runtime.Remoting.Lifetime.Lease:Activate ()>
IL_0031: ret       
IL_0032: call      <System.Runtime.Remoting.Lifetime.Lease:Activate ()>
IL_0037: ret       
// (wrapper remoting-invoke-with-check) System.Threading.Timer:.ctor (System.Threading.TimerCallback,object,System.TimeSpan,System.TimeSpan)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.0   
IL_0007: mono_objaddr
IL_0009: ldind.i   
IL_000a: ldc.i4.0  
IL_000b: add       
IL_000c: ldind.i   
IL_000d: mono_classconst <TransparentProxy>
IL_0013: bne.un    IL_001e
IL_0018: call      <(wrapper remoting-invoke) System.Threading.Timer:.ctor (System.Threading.TimerCallback,object,System.TimeSpan,System.TimeSpan)>
IL_001d: ret       
IL_001e: call      <System.Threading.Timer:.ctor (System.Threading.TimerCallback,object,System.TimeSpan,System.TimeSpan)>
IL_0023: ret       
// (wrapper managed-to-native) System.Threading.Thread:Thread_init ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// (wrapper managed-to-native) System.Threading.Thread:SetState (System.Threading.ThreadState)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <ThreadState>
IL_0019: ret       
// (wrapper managed-to-native) System.Threading.Thread:Thread_internal (System.MulticastDelegate)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <MulticastDelegate>
IL_0019: ret       
// (wrapper managed-to-native) System.Threading.Thread:SetName_internal (string)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <string>
IL_0019: ret       
// (wrapper managed-to-native) System.DateTime:GetTimeMonotonic ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.Threading.Monitor:Monitor_test_synchronised (object)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <object>
IL_000c: ret       
// (wrapper managed-to-native) System.Threading.Monitor:Monitor_wait (object,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <object,int>
IL_000d: ret       
// (wrapper managed-to-native) System.Threading.Monitor:Monitor_pulse (object)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <object>
IL_000c: ret       
// (wrapper managed-to-native) System.Threading.Thread:GetDomainID ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.MonoType:GetInterfaces ()
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: mono_ldptr3
IL_0013: calli     <>
IL_0018: ret       
// TypeInfo__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x0100000e>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// TypeInfo__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// TypeInfo__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    3
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.2   
IL_0017: ldstr     
IL_001c: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0021: ldarg.2   
IL_0022: ldstr     
IL_0027: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002c: ldarg.3   
IL_002d: brfalse   IL_004a
IL_0032: ldarg.2   
IL_0033: ldc.i4.s  1
IL_0035: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_003a: ldarg.2   
IL_003b: ldc.i4.s  6
IL_003d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0042: ldarg.2   
IL_0043: ldc.i4.s  6
IL_0045: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_004a: ret       
// TypeInfo__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <TypeInfo>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x0100000d>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Runtime.Remoting.TypeInfo:serverType>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ldarg.1   
IL_001f: ldarg.2   
IL_0020: ldtoken   <0x1b000002>
IL_0025: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_002a: ldloc.0   
IL_002b: ldfld     <System.Runtime.Remoting.TypeInfo:serverHierarchy>
IL_0030: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0035: ldarg.1   
IL_0036: ldarg.2   
IL_0037: ldtoken   <0x1b000002>
IL_003c: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0041: ldloc.0   
IL_0042: ldfld     <System.Runtime.Remoting.TypeInfo:interfacesImplemented>
IL_0047: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_004c: ret       
// ChannelInfo__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x0100000f>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// ChannelInfo__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// ChannelInfo__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    1
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.3   
IL_0017: brfalse   IL_0024
IL_001c: ldarg.2   
IL_001d: ldc.i4.s  5
IL_001f: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0024: ret       
// ChannelInfo__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <ChannelInfo>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x1b000001>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Runtime.Remoting.ChannelInfo:channelData>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ret       
// GenericEqualityComparer`1__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x1b000003>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// GenericEqualityComparer`1__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// GenericEqualityComparer`1__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    0
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.3   
IL_000c: brfalse   IL_0011
IL_0011: ret       
// GenericEqualityComparer`1__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <GenericEqualityComparer`1>
IL_0006: stloc.0   
IL_0007: ret       
// CrossAppDomainData__TypeMetadata:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldtoken   <0x01000011>
IL_0006: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_000b: call      <System.Runtime.Serialization.Formatters.Binary.ClrTypeMetadata:.ctor (System.Type)>
IL_0010: ret       
// CrossAppDomainData__TypeMetadata:WriteAssemblies (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter)
IL_0000: ret       
// CrossAppDomainData__TypeMetadata:WriteTypeData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,bool)
IL_0000: ldarg.2   
IL_0001: ldc.i4    3
IL_0006: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_000b: ldarg.2   
IL_000c: ldstr     
IL_0011: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0016: ldarg.2   
IL_0017: ldstr     
IL_001c: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_0021: ldarg.2   
IL_0022: ldstr     
IL_0027: callvirt  <System.IO.BinaryWriter:Write (string)>
IL_002c: ldarg.3   
IL_002d: brfalse   IL_0052
IL_0032: ldarg.2   
IL_0033: ldc.i4.s  2
IL_0035: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_003a: ldarg.2   
IL_003b: ldc.i4.s  0
IL_003d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0042: ldarg.2   
IL_0043: ldc.i4.s  1
IL_0045: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_004a: ldarg.2   
IL_004b: ldc.i4.s  8
IL_004d: callvirt  <System.IO.BinaryWriter:Write (byte)>
IL_0052: ret       
// CrossAppDomainData__TypeMetadata:WriteObjectData (System.Runtime.Serialization.Formatters.Binary.ObjectWriter,System.IO.BinaryWriter,object)
IL_0000: ldarg.3   
IL_0001: castclass <CrossAppDomainData>
IL_0006: stloc.0   
IL_0007: ldarg.1   
IL_0008: ldarg.2   
IL_0009: ldtoken   <0x01000008>
IL_000e: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0013: ldloc.0   
IL_0014: ldfld     <System.Runtime.Remoting.Channels.CrossAppDomainData:_ContextID>
IL_0019: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_001e: ldarg.2   
IL_001f: ldloc.0   
IL_0020: ldfld     <System.Runtime.Remoting.Channels.CrossAppDomainData:_DomainID>
IL_0025: callvirt  <System.IO.BinaryWriter:Write (int)>
IL_002a: ldarg.1   
IL_002b: ldarg.2   
IL_002c: ldtoken   <0x0100000d>
IL_0031: call      <System.Type:GetTypeFromHandle (System.RuntimeTypeHandle)>
IL_0036: ldloc.0   
IL_0037: ldfld     <System.Runtime.Remoting.Channels.CrossAppDomainData:_processGuid>
IL_003c: call      <System.Runtime.Serialization.Formatters.Binary.ObjectWriter:WriteValue (System.IO.BinaryWriter,System.Type,object)>
IL_0041: ret       
// (wrapper xdomain-dispatch) Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (object,byte[]&,byte[]&,string,string)
IL_0000: ldnull    
IL_0001: call      <System.Runtime.Remoting.Messaging.CallContext:SetCurrentCallContext (System.Runtime.Remoting.Messaging.LogicalCallContext)>
IL_0006: pop       
IL_0007: ldarg.1   
IL_0008: ldind.ref 
IL_0009: dup       
IL_000a: brfalse.s IL_001d
IL_000c: mono_icall <mono_marshal_xdomain_copy_value>
IL_0012: castclass <Byte[]>
IL_0017: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_001c: stloc.1   
IL_001d: ldarg.0   
IL_001e: call      <System.Runtime.Remoting.Proxies.RealProxy:GetAppDomainTarget ()>
IL_0023: ldarg.3   
IL_0024: mono_icall <mono_marshal_xdomain_copy_value>
IL_002a: castclass <String>
IL_002f: ldarg.s   4
IL_0031: mono_icall <mono_marshal_xdomain_copy_value>
IL_0037: castclass <String>
IL_003c: ldloc.1   
IL_003d: ldc.i4.0  
IL_003e: ldelem.ref
IL_003f: castclass <TextWriter>
IL_0044: ldloc.1   
IL_0045: ldc.i4.1  
IL_0046: ldelem.ref
IL_0047: castclass <Dictionary`2>
IL_004c: mono_ldptr13
IL_0052: ldind.u4  
IL_0053: brfalse   IL_0060
IL_0058: mono_not_taken
IL_005a: mono_icall <mono_thread_force_interruption_checkpoint>
IL_0060: callvirt  <Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_0065: ldarg.1   
IL_0066: ldnull    
IL_0067: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_006c: stind.ref 
IL_006d: ldarg.2   
IL_006e: ldnull    
IL_006f: stind.ref 
IL_0070: leave     IL_0083
IL_0075: call      <System.Runtime.Remoting.RemotingServices:SerializeExceptionData (System.Exception)>
IL_007a: stloc.0   
IL_007b: ldarg.2   
IL_007c: ldloc.0   
IL_007d: stind.ref 
IL_007e: leave     IL_0083
IL_0083: ret       
// (wrapper managed-to-native) System.Runtime.Remoting.Proxies.RealProxy:InternalGetTransparentProxy (string)
IL_0000: ldarg.0   
IL_0001: brtrue    IL_000c
IL_0006: newobj    <System.NullReferenceException:.ctor ()>
IL_000b: throw     
IL_000c: ldarg.0   
IL_000d: ldarg.1   
IL_000e: mono_ldptr3
IL_0014: calli     <string>
IL_0019: ret       
// (wrapper xdomain-invoke) object:ToString ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) object:ToString ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <String>
IL_00b3: ret       
// (wrapper xdomain-invoke) object:GetHashCode ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) object:GetHashCode ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: ret       
// (wrapper xdomain-invoke) object:Finalize ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) object:Finalize ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: dup       
IL_0090: brfalse.s IL_009d
IL_0092: mono_icall <mono_marshal_xdomain_copy_value>
IL_0098: castclass <Byte[]>
IL_009d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a2: pop       
IL_00a3: ret       
// (wrapper xdomain-invoke) object:Equals (object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   7
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   5
IL_0021: ldloc.s   5
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) object:Equals (object)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   5
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.s   4
IL_0056: ldloc.2   
IL_0057: ldloca.s  1
IL_0059: ldloca.s  6
IL_005b: mono_ldptr10
IL_0061: mono_icall <mono_compile_method>
IL_0067: calli     <object,byte[]&,byte[]&>
IL_006c: stloc.3   
IL_006d: ldloc.s   4
IL_006f: ldc.i4.0  
IL_0070: mono_icall <mono_marshal_set_domain_by_id>
IL_0076: pop       
IL_0077: ldloc.s   7
IL_0079: mono_icall <mono_context_set>
IL_007f: ldloc.s   6
IL_0081: brfalse.s IL_00a0
IL_0083: ldloc.s   6
IL_0085: mono_icall <mono_marshal_xdomain_copy_value>
IL_008b: castclass <Byte[]>
IL_0090: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0095: castclass <Exception>
IL_009a: call      <System.Exception:FixRemotingException ()>
IL_009f: throw     
IL_00a0: ldloc.1   
IL_00a1: dup       
IL_00a2: brfalse.s IL_00af
IL_00a4: mono_icall <mono_marshal_xdomain_copy_value>
IL_00aa: castclass <Byte[]>
IL_00af: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b4: pop       
IL_00b5: ldloc.3   
IL_00b6: ret       
// (wrapper xdomain-invoke) System.MarshalByRefObject:InitializeLifetimeService ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.MarshalByRefObject:InitializeLifetimeService ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: ret       
// (wrapper xdomain-invoke) System.MarshalByRefObject:CreateObjRef (System.Type)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.MarshalByRefObject:CreateObjRef (System.Type)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a4: castclass <Byte[]>
IL_00a9: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00ae: castclass <ObjRef>
IL_00b3: ret       
// (wrapper xdomain-invoke) System.IO.TextWriter:Dispose ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.IO.TextWriter:Dispose ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: dup       
IL_0090: brfalse.s IL_009d
IL_0092: mono_icall <mono_marshal_xdomain_copy_value>
IL_0098: castclass <Byte[]>
IL_009d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a2: pop       
IL_00a3: ret       
// (wrapper remoting-invoke-with-check) System.IO.TextWriter/NullTextWriter:.ctor ()
IL_0000: ldarg.0   
IL_0001: ldarg.0   
IL_0002: mono_objaddr
IL_0004: ldind.i   
IL_0005: ldc.i4.0  
IL_0006: add       
IL_0007: ldind.i   
IL_0008: mono_classconst <TransparentProxy>
IL_000e: bne.un    IL_0019
IL_0013: call      <(wrapper remoting-invoke) System.IO.TextWriter/NullTextWriter:.ctor ()>
IL_0018: ret       
IL_0019: call      <System.IO.TextWriter/NullTextWriter:.ctor ()>
IL_001e: ret       
// (wrapper managed-to-native) System.Environment:get_NewLine ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (string,object,object,object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003d
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (string,object,object,object)>
IL_003c: ret       
IL_003d: ldc.i4.3  
IL_003e: newarr    <0x00000007>
IL_0043: dup       
IL_0044: ldc.i4.0  
IL_0045: ldarg.2   
IL_0046: stelem.ref
IL_0047: dup       
IL_0048: ldc.i4.1  
IL_0049: ldarg.3   
IL_004a: stelem.ref
IL_004b: dup       
IL_004c: ldc.i4.2  
IL_004d: ldarg.s   4
IL_004f: stelem.ref
IL_0050: stloc.0   
IL_0051: ldloc.0   
IL_0052: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0057: stloc.1   
IL_0058: ldloc.s   4
IL_005a: ldc.i4.1  
IL_005b: mono_icall <mono_marshal_set_domain_by_id>
IL_0061: stloc.3   
IL_0062: ldloc.2   
IL_0063: ldloca.s  1
IL_0065: ldloca.s  5
IL_0067: ldarg.1   
IL_0068: mono_ldptr10
IL_006e: mono_icall <mono_compile_method>
IL_0074: calli     <object,byte[]&,byte[]&,string>
IL_0079: ldloc.3   
IL_007a: ldc.i4.0  
IL_007b: mono_icall <mono_marshal_set_domain_by_id>
IL_0081: pop       
IL_0082: ldloc.s   6
IL_0084: mono_icall <mono_context_set>
IL_008a: ldloc.s   5
IL_008c: brfalse.s IL_00ab
IL_008e: ldloc.s   5
IL_0090: mono_icall <mono_marshal_xdomain_copy_value>
IL_0096: castclass <Byte[]>
IL_009b: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a0: castclass <Exception>
IL_00a5: call      <System.Exception:FixRemotingException ()>
IL_00aa: throw     
IL_00ab: ldloc.1   
IL_00ac: dup       
IL_00ad: brfalse.s IL_00ba
IL_00af: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b5: castclass <Byte[]>
IL_00ba: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00bf: pop       
IL_00c0: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (string,object,object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003b
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (string,object,object)>
IL_003a: ret       
IL_003b: ldc.i4.2  
IL_003c: newarr    <0x00000007>
IL_0041: dup       
IL_0042: ldc.i4.0  
IL_0043: ldarg.2   
IL_0044: stelem.ref
IL_0045: dup       
IL_0046: ldc.i4.1  
IL_0047: ldarg.3   
IL_0048: stelem.ref
IL_0049: stloc.0   
IL_004a: ldloc.0   
IL_004b: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0050: stloc.1   
IL_0051: ldloc.s   4
IL_0053: ldc.i4.1  
IL_0054: mono_icall <mono_marshal_set_domain_by_id>
IL_005a: stloc.3   
IL_005b: ldloc.2   
IL_005c: ldloca.s  1
IL_005e: ldloca.s  5
IL_0060: ldarg.1   
IL_0061: mono_ldptr10
IL_0067: mono_icall <mono_compile_method>
IL_006d: calli     <object,byte[]&,byte[]&,string>
IL_0072: ldloc.3   
IL_0073: ldc.i4.0  
IL_0074: mono_icall <mono_marshal_set_domain_by_id>
IL_007a: pop       
IL_007b: ldloc.s   6
IL_007d: mono_icall <mono_context_set>
IL_0083: ldloc.s   5
IL_0085: brfalse.s IL_00a4
IL_0087: ldloc.s   5
IL_0089: mono_icall <mono_marshal_xdomain_copy_value>
IL_008f: castclass <Byte[]>
IL_0094: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0099: castclass <Exception>
IL_009e: call      <System.Exception:FixRemotingException ()>
IL_00a3: throw     
IL_00a4: ldloc.1   
IL_00a5: dup       
IL_00a6: brfalse.s IL_00b3
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <Byte[]>
IL_00b3: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b8: pop       
IL_00b9: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (char[],int,int)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0039
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: ldarg.2   
IL_0032: ldarg.3   
IL_0033: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (char[],int,int)>
IL_0038: ret       
IL_0039: ldnull    
IL_003a: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003f: stloc.0   
IL_0040: ldloc.3   
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.2   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  4
IL_004e: ldarg.1   
IL_004f: ldarg.2   
IL_0050: ldarg.3   
IL_0051: mono_ldptr9
IL_0057: mono_icall <mono_compile_method>
IL_005d: calli     <object,byte[]&,byte[]&,char[],int,int>
IL_0062: ldloc.2   
IL_0063: ldc.i4.0  
IL_0064: mono_icall <mono_marshal_set_domain_by_id>
IL_006a: pop       
IL_006b: ldloc.s   5
IL_006d: mono_icall <mono_context_set>
IL_0073: ldloc.s   4
IL_0075: brfalse.s IL_0094
IL_0077: ldloc.s   4
IL_0079: mono_icall <mono_marshal_xdomain_copy_value>
IL_007f: castclass <Byte[]>
IL_0084: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0089: castclass <Exception>
IL_008e: call      <System.Exception:FixRemotingException ()>
IL_0093: throw     
IL_0094: ldloc.0   
IL_0095: dup       
IL_0096: brfalse.s IL_00a3
IL_0098: mono_icall <mono_marshal_xdomain_copy_value>
IL_009e: castclass <Byte[]>
IL_00a3: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a8: pop       
IL_00a9: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (string,object[])
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003a
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (string,object[])>
IL_0039: ret       
IL_003a: ldc.i4.1  
IL_003b: newarr    <0x00000007>
IL_0040: dup       
IL_0041: ldc.i4.0  
IL_0042: ldarg.2   
IL_0043: stelem.ref
IL_0044: stloc.0   
IL_0045: ldloc.0   
IL_0046: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004b: stloc.1   
IL_004c: ldloc.s   4
IL_004e: ldc.i4.1  
IL_004f: mono_icall <mono_marshal_set_domain_by_id>
IL_0055: stloc.3   
IL_0056: ldloc.2   
IL_0057: ldloca.s  1
IL_0059: ldloca.s  5
IL_005b: ldarg.1   
IL_005c: mono_ldptr10
IL_0062: mono_icall <mono_compile_method>
IL_0068: calli     <object,byte[]&,byte[]&,string>
IL_006d: ldloc.3   
IL_006e: ldc.i4.0  
IL_006f: mono_icall <mono_marshal_set_domain_by_id>
IL_0075: pop       
IL_0076: ldloc.s   6
IL_0078: mono_icall <mono_context_set>
IL_007e: ldloc.s   5
IL_0080: brfalse.s IL_009f
IL_0082: ldloc.s   5
IL_0084: mono_icall <mono_marshal_xdomain_copy_value>
IL_008a: castclass <Byte[]>
IL_008f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0094: castclass <Exception>
IL_0099: call      <System.Exception:FixRemotingException ()>
IL_009e: throw     
IL_009f: ldloc.1   
IL_00a0: dup       
IL_00a1: brfalse.s IL_00ae
IL_00a3: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a9: castclass <Byte[]>
IL_00ae: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b3: pop       
IL_00b4: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (string,object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003a
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (string,object)>
IL_0039: ret       
IL_003a: ldc.i4.1  
IL_003b: newarr    <0x00000007>
IL_0040: dup       
IL_0041: ldc.i4.0  
IL_0042: ldarg.2   
IL_0043: stelem.ref
IL_0044: stloc.0   
IL_0045: ldloc.0   
IL_0046: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004b: stloc.1   
IL_004c: ldloc.s   4
IL_004e: ldc.i4.1  
IL_004f: mono_icall <mono_marshal_set_domain_by_id>
IL_0055: stloc.3   
IL_0056: ldloc.2   
IL_0057: ldloca.s  1
IL_0059: ldloca.s  5
IL_005b: ldarg.1   
IL_005c: mono_ldptr10
IL_0062: mono_icall <mono_compile_method>
IL_0068: calli     <object,byte[]&,byte[]&,string>
IL_006d: ldloc.3   
IL_006e: ldc.i4.0  
IL_006f: mono_icall <mono_marshal_set_domain_by_id>
IL_0075: pop       
IL_0076: ldloc.s   6
IL_0078: mono_icall <mono_context_set>
IL_007e: ldloc.s   5
IL_0080: brfalse.s IL_009f
IL_0082: ldloc.s   5
IL_0084: mono_icall <mono_marshal_xdomain_copy_value>
IL_008a: castclass <Byte[]>
IL_008f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0094: castclass <Exception>
IL_0099: call      <System.Exception:FixRemotingException ()>
IL_009e: throw     
IL_009f: ldloc.1   
IL_00a0: dup       
IL_00a1: brfalse.s IL_00ae
IL_00a3: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a9: castclass <Byte[]>
IL_00ae: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b3: pop       
IL_00b4: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (ulong)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (ulong)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,ulong>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (uint)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (uint)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,uint>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (string)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,string>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (single)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (single)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,single>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (object)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (long)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (long)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,long>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (int)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (int)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,int>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (double)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (double)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,double>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (System.Decimal)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (System.Decimal)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: box       <0x00000008>
IL_0047: stelem.ref
IL_0048: stloc.0   
IL_0049: ldloc.0   
IL_004a: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004f: stloc.1   
IL_0050: ldloc.s   4
IL_0052: ldc.i4.1  
IL_0053: mono_icall <mono_marshal_set_domain_by_id>
IL_0059: stloc.3   
IL_005a: ldloc.2   
IL_005b: ldloca.s  1
IL_005d: ldloca.s  5
IL_005f: mono_ldptr11
IL_0065: mono_icall <mono_compile_method>
IL_006b: calli     <object,byte[]&,byte[]&>
IL_0070: ldloc.3   
IL_0071: ldc.i4.0  
IL_0072: mono_icall <mono_marshal_set_domain_by_id>
IL_0078: pop       
IL_0079: ldloc.s   6
IL_007b: mono_icall <mono_context_set>
IL_0081: ldloc.s   5
IL_0083: brfalse.s IL_00a2
IL_0085: ldloc.s   5
IL_0087: mono_icall <mono_marshal_xdomain_copy_value>
IL_008d: castclass <Byte[]>
IL_0092: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0097: castclass <Exception>
IL_009c: call      <System.Exception:FixRemotingException ()>
IL_00a1: throw     
IL_00a2: ldloc.1   
IL_00a3: dup       
IL_00a4: brfalse.s IL_00b1
IL_00a6: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ac: castclass <Byte[]>
IL_00b1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b6: pop       
IL_00b7: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (char[])
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (char[])>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,char[]>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (char)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (char)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,char>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine (bool)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine (bool)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,bool>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:WriteLine ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:WriteLine ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: dup       
IL_0090: brfalse.s IL_009d
IL_0092: mono_icall <mono_marshal_xdomain_copy_value>
IL_0098: castclass <Byte[]>
IL_009d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a2: pop       
IL_00a3: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (string,object,object,object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003d
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: ldarg.s   4
IL_0037: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (string,object,object,object)>
IL_003c: ret       
IL_003d: ldc.i4.3  
IL_003e: newarr    <0x00000007>
IL_0043: dup       
IL_0044: ldc.i4.0  
IL_0045: ldarg.2   
IL_0046: stelem.ref
IL_0047: dup       
IL_0048: ldc.i4.1  
IL_0049: ldarg.3   
IL_004a: stelem.ref
IL_004b: dup       
IL_004c: ldc.i4.2  
IL_004d: ldarg.s   4
IL_004f: stelem.ref
IL_0050: stloc.0   
IL_0051: ldloc.0   
IL_0052: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0057: stloc.1   
IL_0058: ldloc.s   4
IL_005a: ldc.i4.1  
IL_005b: mono_icall <mono_marshal_set_domain_by_id>
IL_0061: stloc.3   
IL_0062: ldloc.2   
IL_0063: ldloca.s  1
IL_0065: ldloca.s  5
IL_0067: ldarg.1   
IL_0068: mono_ldptr10
IL_006e: mono_icall <mono_compile_method>
IL_0074: calli     <object,byte[]&,byte[]&,string>
IL_0079: ldloc.3   
IL_007a: ldc.i4.0  
IL_007b: mono_icall <mono_marshal_set_domain_by_id>
IL_0081: pop       
IL_0082: ldloc.s   6
IL_0084: mono_icall <mono_context_set>
IL_008a: ldloc.s   5
IL_008c: brfalse.s IL_00ab
IL_008e: ldloc.s   5
IL_0090: mono_icall <mono_marshal_xdomain_copy_value>
IL_0096: castclass <Byte[]>
IL_009b: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a0: castclass <Exception>
IL_00a5: call      <System.Exception:FixRemotingException ()>
IL_00aa: throw     
IL_00ab: ldloc.1   
IL_00ac: dup       
IL_00ad: brfalse.s IL_00ba
IL_00af: mono_icall <mono_marshal_xdomain_copy_value>
IL_00b5: castclass <Byte[]>
IL_00ba: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00bf: pop       
IL_00c0: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (string,object,object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003b
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: ldarg.3   
IL_0035: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (string,object,object)>
IL_003a: ret       
IL_003b: ldc.i4.2  
IL_003c: newarr    <0x00000007>
IL_0041: dup       
IL_0042: ldc.i4.0  
IL_0043: ldarg.2   
IL_0044: stelem.ref
IL_0045: dup       
IL_0046: ldc.i4.1  
IL_0047: ldarg.3   
IL_0048: stelem.ref
IL_0049: stloc.0   
IL_004a: ldloc.0   
IL_004b: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_0050: stloc.1   
IL_0051: ldloc.s   4
IL_0053: ldc.i4.1  
IL_0054: mono_icall <mono_marshal_set_domain_by_id>
IL_005a: stloc.3   
IL_005b: ldloc.2   
IL_005c: ldloca.s  1
IL_005e: ldloca.s  5
IL_0060: ldarg.1   
IL_0061: mono_ldptr10
IL_0067: mono_icall <mono_compile_method>
IL_006d: calli     <object,byte[]&,byte[]&,string>
IL_0072: ldloc.3   
IL_0073: ldc.i4.0  
IL_0074: mono_icall <mono_marshal_set_domain_by_id>
IL_007a: pop       
IL_007b: ldloc.s   6
IL_007d: mono_icall <mono_context_set>
IL_0083: ldloc.s   5
IL_0085: brfalse.s IL_00a4
IL_0087: ldloc.s   5
IL_0089: mono_icall <mono_marshal_xdomain_copy_value>
IL_008f: castclass <Byte[]>
IL_0094: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0099: castclass <Exception>
IL_009e: call      <System.Exception:FixRemotingException ()>
IL_00a3: throw     
IL_00a4: ldloc.1   
IL_00a5: dup       
IL_00a6: brfalse.s IL_00b3
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <Byte[]>
IL_00b3: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b8: pop       
IL_00b9: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (char[],int,int)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0039
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: ldarg.2   
IL_0032: ldarg.3   
IL_0033: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (char[],int,int)>
IL_0038: ret       
IL_0039: ldnull    
IL_003a: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003f: stloc.0   
IL_0040: ldloc.3   
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.2   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  4
IL_004e: ldarg.1   
IL_004f: ldarg.2   
IL_0050: ldarg.3   
IL_0051: mono_ldptr9
IL_0057: mono_icall <mono_compile_method>
IL_005d: calli     <object,byte[]&,byte[]&,char[],int,int>
IL_0062: ldloc.2   
IL_0063: ldc.i4.0  
IL_0064: mono_icall <mono_marshal_set_domain_by_id>
IL_006a: pop       
IL_006b: ldloc.s   5
IL_006d: mono_icall <mono_context_set>
IL_0073: ldloc.s   4
IL_0075: brfalse.s IL_0094
IL_0077: ldloc.s   4
IL_0079: mono_icall <mono_marshal_xdomain_copy_value>
IL_007f: castclass <Byte[]>
IL_0084: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0089: castclass <Exception>
IL_008e: call      <System.Exception:FixRemotingException ()>
IL_0093: throw     
IL_0094: ldloc.0   
IL_0095: dup       
IL_0096: brfalse.s IL_00a3
IL_0098: mono_icall <mono_marshal_xdomain_copy_value>
IL_009e: castclass <Byte[]>
IL_00a3: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a8: pop       
IL_00a9: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (string,object[])
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003a
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (string,object[])>
IL_0039: ret       
IL_003a: ldc.i4.1  
IL_003b: newarr    <0x00000007>
IL_0040: dup       
IL_0041: ldc.i4.0  
IL_0042: ldarg.2   
IL_0043: stelem.ref
IL_0044: stloc.0   
IL_0045: ldloc.0   
IL_0046: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004b: stloc.1   
IL_004c: ldloc.s   4
IL_004e: ldc.i4.1  
IL_004f: mono_icall <mono_marshal_set_domain_by_id>
IL_0055: stloc.3   
IL_0056: ldloc.2   
IL_0057: ldloca.s  1
IL_0059: ldloca.s  5
IL_005b: ldarg.1   
IL_005c: mono_ldptr10
IL_0062: mono_icall <mono_compile_method>
IL_0068: calli     <object,byte[]&,byte[]&,string>
IL_006d: ldloc.3   
IL_006e: ldc.i4.0  
IL_006f: mono_icall <mono_marshal_set_domain_by_id>
IL_0075: pop       
IL_0076: ldloc.s   6
IL_0078: mono_icall <mono_context_set>
IL_007e: ldloc.s   5
IL_0080: brfalse.s IL_009f
IL_0082: ldloc.s   5
IL_0084: mono_icall <mono_marshal_xdomain_copy_value>
IL_008a: castclass <Byte[]>
IL_008f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0094: castclass <Exception>
IL_0099: call      <System.Exception:FixRemotingException ()>
IL_009e: throw     
IL_009f: ldloc.1   
IL_00a0: dup       
IL_00a1: brfalse.s IL_00ae
IL_00a3: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a9: castclass <Byte[]>
IL_00ae: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b3: pop       
IL_00b4: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (string,object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_003a
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: ldarg.2   
IL_0034: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (string,object)>
IL_0039: ret       
IL_003a: ldc.i4.1  
IL_003b: newarr    <0x00000007>
IL_0040: dup       
IL_0041: ldc.i4.0  
IL_0042: ldarg.2   
IL_0043: stelem.ref
IL_0044: stloc.0   
IL_0045: ldloc.0   
IL_0046: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004b: stloc.1   
IL_004c: ldloc.s   4
IL_004e: ldc.i4.1  
IL_004f: mono_icall <mono_marshal_set_domain_by_id>
IL_0055: stloc.3   
IL_0056: ldloc.2   
IL_0057: ldloca.s  1
IL_0059: ldloca.s  5
IL_005b: ldarg.1   
IL_005c: mono_ldptr10
IL_0062: mono_icall <mono_compile_method>
IL_0068: calli     <object,byte[]&,byte[]&,string>
IL_006d: ldloc.3   
IL_006e: ldc.i4.0  
IL_006f: mono_icall <mono_marshal_set_domain_by_id>
IL_0075: pop       
IL_0076: ldloc.s   6
IL_0078: mono_icall <mono_context_set>
IL_007e: ldloc.s   5
IL_0080: brfalse.s IL_009f
IL_0082: ldloc.s   5
IL_0084: mono_icall <mono_marshal_xdomain_copy_value>
IL_008a: castclass <Byte[]>
IL_008f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0094: castclass <Exception>
IL_0099: call      <System.Exception:FixRemotingException ()>
IL_009e: throw     
IL_009f: ldloc.1   
IL_00a0: dup       
IL_00a1: brfalse.s IL_00ae
IL_00a3: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a9: castclass <Byte[]>
IL_00ae: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b3: pop       
IL_00b4: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (ulong)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (ulong)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,ulong>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (uint)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (uint)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,uint>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (string)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,string>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (single)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (single)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,single>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (object)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (object)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: stelem.ref
IL_0043: stloc.0   
IL_0044: ldloc.0   
IL_0045: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004a: stloc.1   
IL_004b: ldloc.s   4
IL_004d: ldc.i4.1  
IL_004e: mono_icall <mono_marshal_set_domain_by_id>
IL_0054: stloc.3   
IL_0055: ldloc.2   
IL_0056: ldloca.s  1
IL_0058: ldloca.s  5
IL_005a: mono_ldptr10
IL_0060: mono_icall <mono_compile_method>
IL_0066: calli     <object,byte[]&,byte[]&>
IL_006b: ldloc.3   
IL_006c: ldc.i4.0  
IL_006d: mono_icall <mono_marshal_set_domain_by_id>
IL_0073: pop       
IL_0074: ldloc.s   6
IL_0076: mono_icall <mono_context_set>
IL_007c: ldloc.s   5
IL_007e: brfalse.s IL_009d
IL_0080: ldloc.s   5
IL_0082: mono_icall <mono_marshal_xdomain_copy_value>
IL_0088: castclass <Byte[]>
IL_008d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0092: castclass <Exception>
IL_0097: call      <System.Exception:FixRemotingException ()>
IL_009c: throw     
IL_009d: ldloc.1   
IL_009e: dup       
IL_009f: brfalse.s IL_00ac
IL_00a1: mono_icall <mono_marshal_xdomain_copy_value>
IL_00a7: castclass <Byte[]>
IL_00ac: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b1: pop       
IL_00b2: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (long)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (long)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,long>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (int)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (int)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,int>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.TextWriter:Write (double)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.TextWriter:Write (double)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,double>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (System.Decimal)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.2   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0039
IL_0031: ldarg.0   
IL_0032: ldarg.1   
IL_0033: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (System.Decimal)>
IL_0038: ret       
IL_0039: ldc.i4.1  
IL_003a: newarr    <0x00000007>
IL_003f: dup       
IL_0040: ldc.i4.0  
IL_0041: ldarg.1   
IL_0042: box       <0x00000008>
IL_0047: stelem.ref
IL_0048: stloc.0   
IL_0049: ldloc.0   
IL_004a: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_004f: stloc.1   
IL_0050: ldloc.s   4
IL_0052: ldc.i4.1  
IL_0053: mono_icall <mono_marshal_set_domain_by_id>
IL_0059: stloc.3   
IL_005a: ldloc.2   
IL_005b: ldloca.s  1
IL_005d: ldloca.s  5
IL_005f: mono_ldptr11
IL_0065: mono_icall <mono_compile_method>
IL_006b: calli     <object,byte[]&,byte[]&>
IL_0070: ldloc.3   
IL_0071: ldc.i4.0  
IL_0072: mono_icall <mono_marshal_set_domain_by_id>
IL_0078: pop       
IL_0079: ldloc.s   6
IL_007b: mono_icall <mono_context_set>
IL_0081: ldloc.s   5
IL_0083: brfalse.s IL_00a2
IL_0085: ldloc.s   5
IL_0087: mono_icall <mono_marshal_xdomain_copy_value>
IL_008d: castclass <Byte[]>
IL_0092: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0097: castclass <Exception>
IL_009c: call      <System.Exception:FixRemotingException ()>
IL_00a1: throw     
IL_00a2: ldloc.1   
IL_00a3: dup       
IL_00a4: brfalse.s IL_00b1
IL_00a6: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ac: castclass <Byte[]>
IL_00b1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00b6: pop       
IL_00b7: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (char[])
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (char[])>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,char[]>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (char)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (char)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,char>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Write (bool)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Write (bool)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,bool>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Flush ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Flush ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: dup       
IL_0090: brfalse.s IL_009d
IL_0092: mono_icall <mono_marshal_xdomain_copy_value>
IL_0098: castclass <Byte[]>
IL_009d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a2: pop       
IL_00a3: ret       
// (wrapper xdomain-invoke) System.IO.TextWriter:Dispose (bool)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.TextWriter:Dispose (bool)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,bool>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:Close ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:Close ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: dup       
IL_0090: brfalse.s IL_009d
IL_0092: mono_icall <mono_marshal_xdomain_copy_value>
IL_0098: castclass <Byte[]>
IL_009d: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a2: pop       
IL_00a3: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:set_NewLine (string)
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0037
IL_002f: ldarg.0   
IL_0030: ldarg.1   
IL_0031: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:set_NewLine (string)>
IL_0036: ret       
IL_0037: ldnull    
IL_0038: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003d: stloc.0   
IL_003e: ldloc.3   
IL_003f: ldc.i4.1  
IL_0040: mono_icall <mono_marshal_set_domain_by_id>
IL_0046: stloc.2   
IL_0047: ldloc.1   
IL_0048: ldloca.s  0
IL_004a: ldloca.s  4
IL_004c: ldarg.1   
IL_004d: mono_ldptr9
IL_0053: mono_icall <mono_compile_method>
IL_0059: calli     <object,byte[]&,byte[]&,string>
IL_005e: ldloc.2   
IL_005f: ldc.i4.0  
IL_0060: mono_icall <mono_marshal_set_domain_by_id>
IL_0066: pop       
IL_0067: ldloc.s   5
IL_0069: mono_icall <mono_context_set>
IL_006f: ldloc.s   4
IL_0071: brfalse.s IL_0090
IL_0073: ldloc.s   4
IL_0075: mono_icall <mono_marshal_xdomain_copy_value>
IL_007b: castclass <Byte[]>
IL_0080: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0085: castclass <Exception>
IL_008a: call      <System.Exception:FixRemotingException ()>
IL_008f: throw     
IL_0090: ldloc.0   
IL_0091: dup       
IL_0092: brfalse.s IL_009f
IL_0094: mono_icall <mono_marshal_xdomain_copy_value>
IL_009a: castclass <Byte[]>
IL_009f: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a4: pop       
IL_00a5: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:get_NewLine ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   6
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_0031
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.s   4
IL_0021: ldloc.s   4
IL_0023: mono_ldptr4
IL_0029: mono_icall <mono_marshal_check_domain_image>
IL_002f: brtrue.s  IL_0038
IL_0031: ldarg.0   
IL_0032: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:get_NewLine ()>
IL_0037: ret       
IL_0038: ldnull    
IL_0039: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003e: stloc.0   
IL_003f: ldloc.s   4
IL_0041: ldc.i4.1  
IL_0042: mono_icall <mono_marshal_set_domain_by_id>
IL_0048: stloc.3   
IL_0049: ldloc.1   
IL_004a: ldloca.s  0
IL_004c: ldloca.s  5
IL_004e: mono_ldptr9
IL_0054: mono_icall <mono_compile_method>
IL_005a: calli     <object,byte[]&,byte[]&>
IL_005f: stloc.2   
IL_0060: ldloc.3   
IL_0061: ldc.i4.0  
IL_0062: mono_icall <mono_marshal_set_domain_by_id>
IL_0068: pop       
IL_0069: ldloc.s   6
IL_006b: mono_icall <mono_context_set>
IL_0071: ldloc.s   5
IL_0073: brfalse.s IL_0092
IL_0075: ldloc.s   5
IL_0077: mono_icall <mono_marshal_xdomain_copy_value>
IL_007d: castclass <Byte[]>
IL_0082: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0087: castclass <Exception>
IL_008c: call      <System.Exception:FixRemotingException ()>
IL_0091: throw     
IL_0092: ldloc.0   
IL_0093: dup       
IL_0094: brfalse.s IL_00a1
IL_0096: mono_icall <mono_marshal_xdomain_copy_value>
IL_009c: castclass <Byte[]>
IL_00a1: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_00a6: pop       
IL_00a7: ldloc.2   
IL_00a8: mono_icall <mono_marshal_xdomain_copy_value>
IL_00ae: castclass <String>
IL_00b3: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:get_FormatProvider ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:get_FormatProvider ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: castclass <IFormatProvider>
IL_00a4: ret       
// (wrapper xdomain-invoke) System.IO.SynchronizedWriter:get_Encoding ()
IL_0000: mono_icall <mono_context_get>
IL_0006: dup       
IL_0007: stloc.s   5
IL_0009: call      <System.Runtime.Remoting.Contexts.Context:get_NeedsContextSink ()>
IL_000e: brtrue.s  IL_002f
IL_0010: ldarg.0   
IL_0011: mono_objaddr
IL_0013: ldc.i4.s  16
IL_0015: add       
IL_0016: ldind.ref 
IL_0017: dup       
IL_0018: stloc.1   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  40
IL_001d: add       
IL_001e: ldind.i4  
IL_001f: stloc.3   
IL_0020: ldloc.3   
IL_0021: mono_ldptr4
IL_0027: mono_icall <mono_marshal_check_domain_image>
IL_002d: brtrue.s  IL_0036
IL_002f: ldarg.0   
IL_0030: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:get_Encoding ()>
IL_0035: ret       
IL_0036: ldnull    
IL_0037: call      <System.Runtime.Remoting.RemotingServices:SerializeCallData (object)>
IL_003c: stloc.0   
IL_003d: ldloc.3   
IL_003e: ldc.i4.1  
IL_003f: mono_icall <mono_marshal_set_domain_by_id>
IL_0045: stloc.2   
IL_0046: ldloc.1   
IL_0047: ldloca.s  0
IL_0049: ldloca.s  4
IL_004b: mono_ldptr9
IL_0051: mono_icall <mono_compile_method>
IL_0057: calli     <object,byte[]&,byte[]&>
IL_005c: ldloc.2   
IL_005d: ldc.i4.0  
IL_005e: mono_icall <mono_marshal_set_domain_by_id>
IL_0064: pop       
IL_0065: ldloc.s   5
IL_0067: mono_icall <mono_context_set>
IL_006d: ldloc.s   4
IL_006f: brfalse.s IL_008e
IL_0071: ldloc.s   4
IL_0073: mono_icall <mono_marshal_xdomain_copy_value>
IL_0079: castclass <Byte[]>
IL_007e: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_0083: castclass <Exception>
IL_0088: call      <System.Exception:FixRemotingException ()>
IL_008d: throw     
IL_008e: ldloc.0   
IL_008f: mono_icall <mono_marshal_xdomain_copy_value>
IL_0095: castclass <Byte[]>
IL_009a: call      <System.Runtime.Remoting.RemotingServices:DeserializeCallData (byte[])>
IL_009f: castclass <Encoding>
IL_00a4: ret       
// (wrapper managed-to-native) System.Runtime.InteropServices.GCHandle:GetTargetHandle (object,int,System.Runtime.InteropServices.GCHandleType)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: mono_ldptr2
IL_0009: calli     <object,int,GCHandleType>
IL_000e: ret       
// (wrapper proxy_isinst) object:__proxy_isinst_wrapper_IObjectReference (object)
IL_0000: ldarg.0   
IL_0001: mono_objaddr
IL_0003: ldc.i4.s  16
IL_0005: add       
IL_0006: ldind.ref 
IL_0007: mono_ldptr2
IL_000d: mono_icall <type_from_handle>
IL_0013: ldarg.0   
IL_0014: callvirt  <System.Runtime.Remoting.IRemotingTypeInfo:CanCastTo (System.Type,object)>
IL_0019: brfalse   IL_004b
IL_001e: mono_ldptr5
IL_0024: mono_icall <type_from_handle>
IL_002a: ldarg.0   
IL_002b: mono_icall <mono_upgrade_remote_class_wrapper>
IL_0031: mono_ldptr8
IL_0037: ldind.u4  
IL_0038: brfalse   IL_0045
IL_003d: mono_not_taken
IL_003f: mono_icall <mono_thread_interruption_checkpoint>
IL_0045: ldarg.0   
IL_0046: br        IL_004c
IL_004b: ldnull    
IL_004c: ret       
// (wrapper remoting-invoke-with-check) Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.0   
IL_0007: mono_objaddr
IL_0009: ldind.i   
IL_000a: ldc.i4.0  
IL_000b: add       
IL_000c: ldind.i   
IL_000d: mono_classconst <TransparentProxy>
IL_0013: bne.un    IL_0037
IL_0018: ldarg.0   
IL_0019: mono_objaddr
IL_001b: ldc.i4.s  16
IL_001d: add       
IL_001e: ldind.ref 
IL_001f: mono_objaddr
IL_0021: ldc.i4.s  40
IL_0023: add       
IL_0024: ldind.i4  
IL_0025: ldc.i4.m1 
IL_0026: beq       IL_0031
IL_002b: call      <(wrapper xdomain-invoke) Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_0030: ret       
IL_0031: call      <(wrapper remoting-invoke) Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_0036: ret       
IL_0037: call      <Mango.Templates.Minge.MingeAssemblyWrapper:RenderToStream (string,string,System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_003c: ret       
// (wrapper managed-to-native) System.Text.Encoding:InternalCodePage (int&)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <int&>
IL_000c: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_ConsoleError ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper remoting-invoke-with-check) System.IO.FileStream:.ctor (intptr,System.IO.FileAccess,bool,int,bool,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: ldarg.s   5
IL_0008: ldarg.s   6
IL_000a: ldarg.0   
IL_000b: mono_objaddr
IL_000d: ldind.i   
IL_000e: ldc.i4.0  
IL_000f: add       
IL_0010: ldind.i   
IL_0011: mono_classconst <TransparentProxy>
IL_0017: bne.un    IL_0022
IL_001c: call      <(wrapper remoting-invoke) System.IO.FileStream:.ctor (intptr,System.IO.FileAccess,bool,int,bool,bool)>
IL_0021: ret       
IL_0022: call      <System.IO.FileStream:.ctor (intptr,System.IO.FileAccess,bool,int,bool,bool)>
IL_0027: ret       
// (wrapper remoting-invoke-with-check) System.IO.UnexceptionalStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_001b
IL_0015: call      <(wrapper remoting-invoke) System.IO.UnexceptionalStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_001a: ret       
IL_001b: call      <System.IO.UnexceptionalStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_0020: ret       
// (wrapper remoting-invoke-with-check) System.IO.StreamWriter:.ctor (System.IO.Stream,System.Text.Encoding,int)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.0   
IL_0005: mono_objaddr
IL_0007: ldind.i   
IL_0008: ldc.i4.0  
IL_0009: add       
IL_000a: ldind.i   
IL_000b: mono_classconst <TransparentProxy>
IL_0011: bne.un    IL_001c
IL_0016: call      <(wrapper remoting-invoke) System.IO.StreamWriter:.ctor (System.IO.Stream,System.Text.Encoding,int)>
IL_001b: ret       
IL_001c: call      <System.IO.StreamWriter:.ctor (System.IO.Stream,System.Text.Encoding,int)>
IL_0021: ret       
// (wrapper remoting-invoke-with-check) System.IO.SynchronizedWriter:.ctor (System.IO.TextWriter,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_001b
IL_0015: call      <(wrapper remoting-invoke) System.IO.SynchronizedWriter:.ctor (System.IO.TextWriter,bool)>
IL_001a: ret       
IL_001b: call      <System.IO.SynchronizedWriter:.ctor (System.IO.TextWriter,bool)>
IL_0020: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_ConsoleOutput ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper managed-to-native) System.ConsoleDriver:Isatty (intptr)
IL_0000: ldarg.0   
IL_0001: mono_ldptr2
IL_0007: calli     <intptr>
IL_000c: ret       
// (wrapper managed-to-native) System.IO.MonoIO:get_ConsoleInput ()
IL_0000: mono_ldptr2
IL_0006: calli     <>
IL_000b: ret       
// (wrapper remoting-invoke-with-check) System.IO.CStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_001b
IL_0015: call      <(wrapper remoting-invoke) System.IO.CStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_001a: ret       
IL_001b: call      <System.IO.CStreamWriter:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_0020: ret       
// (wrapper remoting-invoke-with-check) System.IO.CStreamReader:.ctor (System.IO.Stream,System.Text.Encoding)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.0   
IL_0004: mono_objaddr
IL_0006: ldind.i   
IL_0007: ldc.i4.0  
IL_0008: add       
IL_0009: ldind.i   
IL_000a: mono_classconst <TransparentProxy>
IL_0010: bne.un    IL_001b
IL_0015: call      <(wrapper remoting-invoke) System.IO.CStreamReader:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_001a: ret       
IL_001b: call      <System.IO.CStreamReader:.ctor (System.IO.Stream,System.Text.Encoding)>
IL_0020: ret       
// (wrapper managed-to-native) System.IO.MonoIO:Write (intptr,byte[],int,int,System.IO.MonoIOError&)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: ldarg.2   
IL_0003: ldarg.3   
IL_0004: ldarg.s   4
IL_0006: mono_ldptr2
IL_000c: calli     <intptr,byte[],int,int,MonoIOError&>
IL_0011: ret       
// (wrapper managed-to-native) System.Reflection.Assembly:LoadFrom (string,bool)
IL_0000: ldarg.0   
IL_0001: ldarg.1   
IL_0002: mono_ldptr2
IL_0008: calli     <string,bool>
IL_000d: ret       
// templates.page_template:.ctor ()
IL_0000: ret       
// templates.page_template:RenderToStream (System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)
IL_0000: nop       
IL_0001: ldarg.0   
IL_0002: ldarg.1   
IL_0003: ldarg.2   
IL_0004: callvirt  <templates.page_base:RenderToStream (System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_0009: ret       
IL_000a: ldarg.1   
IL_000b: ldstr     ""
IL_0010: callvirt  <System.IO.TextWriter:Write (string)>
IL_0015: ldarg.0   
IL_0016: ldarg.1   
IL_0017: ldarg.2   
IL_0018: callvirt  <templates.page_template:some_other_block (System.IO.TextWriter,System.Collections.Generic.Dictionary`2<string, object>)>
IL_001d: ret       
