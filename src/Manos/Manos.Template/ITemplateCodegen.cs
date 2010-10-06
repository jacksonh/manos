
using System;
using System.IO;
using System.Text;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Manos.Templates {

	  public class ITemplateCodegen {

	  	 public void BeginPage (string name)
		 {
		 }

		 public void EndPage ()
		 {
		 }
		 
		 public void EmitExtends (string name)
		 {
		 }

		 public void AddData (string data)
		 {
		 }

		 public void EmitSinglePrint (Expression expression)
		 {
		 }

		 public void EmitIf (Expression expression)
		 {
		 }

		 public void EmitElse (Expression expression)
		 {
		 }

		 public void EmitEndIf ()
		 {
		 }

		 public void BeginBlock (string name)
		 {
		 }

		 public void EndBlock (string name)
		 {
		 }

		 public void BeginForeachLoop (string var, Expression expression)
		 {
		 }

		 public void EndForeachLoop ()
		 {
		 }
	  }
}

