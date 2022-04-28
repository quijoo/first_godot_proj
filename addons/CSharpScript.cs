using System;
using System.Runtime.CompilerServices;
using Godot.Collections;
using Array = Godot.Collections.Array;

/* Example Usage:
  // Declare a class with the attribute
  [CSharpScript]
  public class CustomResource : Resource { ... }
  // Later, create new resources with
  CSharpScript<CustomResource>.New()
  
  // Report issues to the gist at: https://gist.github.com/cgbeutler/c4f00b98d744ac438b84e8840bbe1740
*/

namespace Godot
{
  [AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false )]
  public sealed class CSharpScriptAttribute : Attribute
  {
    public CSharpScriptAttribute( [CallerFilePath] string path = "" )
    {
      FilePath = path;
    }

    public string FilePath { get; set; }
  };


  public static class CSharpScript<T>
    where T : class
  {
    // TODO: Edit this string to be the project-path of THIS file (backlashes and no '.' prefixes)
    private const string __THIS_FILE_PROJ_PATH = "addons/CSharpScript.cs";
    
    private static string __projectPath = null;
    private static string __ProjectPath => __projectPath ??= __InitProjectPath();
    private static string __InitProjectPath( [CallerFilePath] string callerPath = "" )
    {
      callerPath = callerPath.Replace( System.IO.Path.DirectorySeparatorChar, '/' );
      if (!callerPath.EndsWith( "/" + __THIS_FILE_PROJ_PATH ))
      {
        GD.PushError( "Failed to get project path. Project-path of this file may have changed." );
        throw new Exception("Failed to get project path. Project-path of this file may have changed.");
      }
      return callerPath.Remove( callerPath.Length - __THIS_FILE_PROJ_PATH.Length );
    }


    public static string FilePath;
    public static string Filename;

    public static string ResourcePath => FilePath;

    static CSharpScript()
    {
      if (Attribute.GetCustomAttribute( typeof( T ), typeof( CSharpScriptAttribute ) ) is CSharpScriptAttribute attr)
      {
        var tmpFilePath = attr.FilePath.Replace( System.IO.Path.DirectorySeparatorChar, '/' );
        if (!tmpFilePath.BeginsWith( __ProjectPath ))
        {
          GD.PushError( $"Can't get script 'res' path:  Raw path didn't start with project path" );
          FilePath = Filename = "";
          return;
        }
        if (!tmpFilePath.EndsWith( ".cs" ))
        {
          GD.PushError( $"Can't get scritp 'res' path:  Raw path didn't end with '.cs'" );
          FilePath = Filename = "";
          return;
        }
        FilePath = "res://" + tmpFilePath.Substring( __ProjectPath.Length );
        Filename = FilePath.GetFile();
        if (Filename.BaseName() != typeof( T ).Name)
        {
          GD.PushError( $"Class name '{ typeof( T ).Name }' doesn't match filename '{ Filename }'" );
        }
      }
      else
      {
        FilePath = Filename = typeof(T).Name;
        GD.PushError( $"'{nameof( CSharpScriptAttribute )}' missing from the class '{typeof( T ).Name}'." );
      }
    }

    private static WeakRef __csharpScript = null; //<CSharpScript>
    /// Get the CSharpScript Godot Resource
    public static CSharpScript GetCSharpScript()
    {
      if (__csharpScript?.GetRef() is CSharpScript scr) { return scr; }
      scr = GD.Load<CSharpScript>( ResourcePath );
      if (scr != null) { __csharpScript = Godot.Object.WeakRef( scr ); }
      else { throw new Exception( "Can't load CSharp Script" ); }
      return scr;
    }

    /// Returns a new instance of the script.
    public static T New()
    {
      var script = GetCSharpScript();
      try
      {
        var o = script.New();
        var t = (T) o;
        return t;
      }
      catch (Exception e)
      {
        GD.PrintErr( "Exception in New(): " + e.ToString() );
        GD.PrintStack();
      }
      return null!;
    }

    /// Returns the default value of the specified property.
    public static object GetPropertyDefaultValue( string property )
    {
      var script = GetCSharpScript();
      return script.GetPropertyDefaultValue( property );
    }

    /// Returns a dictionary containing constant names and their values.
    public static Dictionary GetScriptConstantMap()
    {
      var script = GetCSharpScript();
      return script.GetScriptConstantMap();
    }

    /// Returns the list of methods in this Godot.Script.
    public static Array GetScriptMethodList()
    {
      var script = GetCSharpScript();
      return script.GetScriptMethodList();
    }

    /// Returns the list of properties in this Godot.Script.
    public static Array GetScriptPropertyList()
    {
      var script = GetCSharpScript();
      return script.GetScriptPropertyList();
    }

    /// Returns the list of user signals defined in this Godot.Script.
    public static Array GetScriptSignalList()
    {
      var script = GetCSharpScript();
      return script.GetScriptSignalList();
    }

    /// Returns true if the script, or a base class, defines a signal with the given
    /// name.
    public static bool HasScriptSignal( string signalName )
    {
      var script = GetCSharpScript();
      return script?.HasScriptSignal( signalName ) ?? false;
    }

    /// Returns true if the script is a tool script. A tool script can run in the editor.
    public static bool IsTool()
    {
      var script = GetCSharpScript();
      return script?.IsTool() ?? false;
    }
  };


  public static class CSharpScriptExt
  {
    public static string ResourcePath( this Type t )
    {
      var sourceInfo = (CSharpScriptAttribute) Attribute.GetCustomAttribute( t, typeof( CSharpScriptAttribute ) );
      if (sourceInfo == null)
      {
        GD.PushError( $"Could not file script info. Did you add '{nameof( CSharpScriptAttribute )}' to the class '{t.Name}'?" );
        return "";
      }
      if (sourceInfo?.FilePath.GetFile().BaseName() != t.Name)
      {
        GD.PushError( $"Class and script name mismatch. Class name is '{ t.Name }' for script '{ sourceInfo?.FilePath }'" );
        return "";
      }
      return sourceInfo?.FilePath ?? "";
    }

    public static CSharpScript AsCSharpScript( this Type t )
    {
      var scriptPath = ResourcePath( t );
      if (scriptPath.Empty()) { throw new Exception( "Can't load CSharp Script" ); }
      // Don't worry, it will usually be a cached load
      // Also, in tool mode it can get scrapped randomly, so we kinda need to use load each time
      return GD.Load<CSharpScript>( scriptPath );
    }

    /// Returns a new instance of the script.
    public static object New( this Type t )
    {
      var script = AsCSharpScript( t );
      // if ( Engine.EditorHint && ! script.IsTool() )
      // { GD.PushWarning($"Script is not in tool mode: '{ typeof( T ).Name }'"); }
      return script.New();
    }

    /// Returns the default value of the specified property.
    public static object GetPropertyDefaultValue( this Type t, string property )
    {
      var script = AsCSharpScript( t );
      return script.GetPropertyDefaultValue( property );
    }

    /// Returns a dictionary containing constant names and their values.
    public static Dictionary GetScriptConstantMap( this Type t )
    {
      var script = AsCSharpScript( t );
      return script.GetScriptConstantMap();
    }

    /// Returns the list of methods in this Godot.Script.
    public static Array GetScriptMethodList( this Type t )
    {
      var script = AsCSharpScript( t );
      return script.GetScriptMethodList();
    }

    /// Returns the list of properties in this Godot.Script.
    public static Array GetScriptPropertyList( this Type t )
    {
      var script = AsCSharpScript( t );
      return script.GetScriptPropertyList();
    }

    /// Returns the list of user signals defined in this Godot.Script.
    public static Array GetScriptSignalList( this Type t )
    {
      var script = AsCSharpScript( t );
      return script.GetScriptSignalList();
    }

    /// Returns true if the script, or a base class, defines a signal with the given
    /// name.
    public static bool HasScriptSignal( this Type t, string signalName )
    {
      var script = AsCSharpScript( t );
      return script?.HasScriptSignal( signalName ) ?? false;
    }

    /// Returns true if the script is a tool script. A tool script can run in the editor.
    public static bool IsTool( this Type t )
    {
      var script = AsCSharpScript( t );
      return script?.IsTool() ?? false;
    }
  };
}