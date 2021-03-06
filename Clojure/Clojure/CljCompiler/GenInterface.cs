﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using clojure.lang.CljCompiler.Ast;

namespace clojure.lang
{
    public static class GenInterface
    {
        #region Factory

        public static Type GenerateInterface(string iName, ISeq extends, ISeq methods)
        {
            
            GenContext context = new GenContext(iName, CompilerMode.File);

//            GenContext context = (GenContext)Compiler.COMPILER_CONTEXT.deref();
//            if (context == null)
//            {
//#if DEBUG
//                context = new GenContext(iName, CompilerMode.File);
//#else
//                throw new InvalidOperationException("No compiler context on the stack.");
//#endif
//            }  

            Type[] interfaceTypes = GenClass.CreateTypeArray(extends == null ? null : extends.seq());

            TypeBuilder proxyTB = context.ModuleBldr.DefineType(
                iName,
                TypeAttributes.Interface | TypeAttributes.Public | TypeAttributes.Abstract,
                null,
                interfaceTypes);


            DefineMethods(proxyTB, methods);

            Type t = proxyTB.CreateType();
            context.AssyBldr.Save(iName  + ".dll");
            return t;
        }

        #endregion

        #region Defining methods

        private static void DefineMethods(TypeBuilder proxyTB, ISeq methods)
        {
            for (ISeq s = methods == null ? null : methods.seq(); s != null; s = s.next())
                DefineMethod(proxyTB, (IPersistentVector)s.first());
        }

        private static void DefineMethod(TypeBuilder proxyTB, IPersistentVector sig)
        {
            string mname = (string)sig.nth(0);
            Type[] paramTypes = GenClass.CreateTypeArray((ISeq)sig.nth(1));
            Type retType = (Type)sig.nth(2);

            MethodBuilder mb = proxyTB.DefineMethod(mname, MethodAttributes.Abstract | MethodAttributes.Public| MethodAttributes.Virtual, retType, paramTypes);
        }

        #endregion
    }
}
