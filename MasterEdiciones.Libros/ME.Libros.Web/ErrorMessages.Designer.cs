﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ME.Libros.Web {
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ME.Libros.Web.ErrorMessages", typeof(ErrorMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El CUIL debe contener 11 caracteres.
        /// </summary>
        public static string CuilLength {
            get {
                return ResourceManager.GetString("CuilLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El &lt;b&gt;CUIL&lt;/b&gt; ingresado ya se encuentra asociado a un cliente existente.
        /// </summary>
        public static string CuilRepetido {
            get {
                return ResourceManager.GetString("CuilRepetido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El cliente no puede ser eliminado porque tiene datos asociados.
        /// </summary>
        public static string EliminarCliente {
            get {
                return ResourceManager.GetString("EliminarCliente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La localidad no puede ser eliminada porque tiene datos asociados.
        /// </summary>
        public static string EliminarLocalidad {
            get {
                return ResourceManager.GetString("EliminarLocalidad", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ha ocurrido un error en el sistema.
        /// </summary>
        public static string ErrorSistema {
            get {
                return ResourceManager.GetString("ErrorSistema", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El valor {0} es requerido.
        /// </summary>
        public static string PropertyValueRequired {
            get {
                return ResourceManager.GetString("PropertyValueRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El &lt;b&gt;Telefono Fijo&lt;/b&gt; debe contener 10 caracteres.
        /// </summary>
        public static string TelefonoFijoLength {
            get {
                return ResourceManager.GetString("TelefonoFijoLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El campo es requerido.
        /// </summary>
        public static string ValidationRequired {
            get {
                return ResourceManager.GetString("ValidationRequired", resourceCulture);
            }
        }
    }
}
