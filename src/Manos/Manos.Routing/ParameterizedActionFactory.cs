

using System;
using System.Reflection;
using System.Reflection.Emit;

//
// Adapted from example code here:
// http://www.codeproject.com/KB/cs/FastInvokerWrapper.aspx
//

namespace Manos.Routing
{
	public static class ParameterizedActionFactory
	{
		public static ParameterizedAction CreateAction (MethodInfo method)
		{
			DynamicMethod dynamic = new DynamicMethod (String.Empty, typeof (void), 
			                                           new Type [] { typeof(object), typeof(object[]) }, 
                          							   method.DeclaringType.Module);
			
			ILGenerator codegen = dynamic.GetILGenerator ();
            ParameterInfo [] parameters = method.GetParameters ();
            
			Type [] param_types = new Type [parameters.Length];
            for (int i = 0; i < param_types.Length; i++)
            {
                if (parameters [i].ParameterType.IsByRef) {
					Console.Error.WriteLine ("By Ref parameters are not allowed in Action signatures.");
                    return null;
				}
                param_types [i] = parameters [i].ParameterType;
            }
           
			LocalBuilder [] locals = new LocalBuilder [param_types.Length];
			for (int i = 0; i < param_types.Length; i++) {
                locals [i] = codegen.DeclareLocal (param_types [i], true);
            }
            
			for (int i = 0; i < param_types.Length; i++) {
                codegen.Emit (OpCodes.Ldarg_1);
                EmitFastInt (codegen, i);
                codegen.Emit (OpCodes.Ldelem_Ref);
                EmitCastToReference (codegen, param_types [i]);
                codegen.Emit (OpCodes.Stloc, locals [i]);
            }
			
			if (!method.IsStatic) {
                codegen.Emit (OpCodes.Ldarg_0);
            }
			
			for (int i = 0; i < param_types.Length; i++) {
                codegen.Emit (OpCodes.Ldloc, locals [i]);
            }
			
			if (method.IsStatic)
                codegen.EmitCall (OpCodes.Call, method, null);
            else
                codegen.EmitCall(OpCodes.Callvirt, method, null);
            
			codegen.Emit (OpCodes.Ret);
            
			ParameterizedAction action = (ParameterizedAction) dynamic.CreateDelegate (typeof(ParameterizedAction));
            return action;
		}
		
		private static void EmitCastToReference (ILGenerator il, Type type)
        {
            if (type.IsValueType)
                il.Emit (OpCodes.Unbox_Any, type);
            else
                il.Emit (OpCodes.Castclass, type);
        }
		
		private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit (OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit (OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit (OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit (OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit (OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit (OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit (OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit (OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit (OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit (OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
                il.Emit (OpCodes.Ldc_I4_S, (SByte) value);
            else
                il.Emit (OpCodes.Ldc_I4, value);
        }
	}
}

