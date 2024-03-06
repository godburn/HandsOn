using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Utility {
    public static void Invoke( this MonoBehaviour mb, System.Action func, float delay ) {
        mb.StartCoroutine( InvokeRoutine( func, delay ) );
    }
    private static IEnumerator InvokeRoutine( System.Action func, float delay ) {
        yield return new WaitForSeconds( delay );
        func();
    }


    public static void CopyGameObjectProps( GameObject from, GameObject to ) {
        RectTransform rtf = from.GetComponent<RectTransform>();
        RectTransform rtt = to.GetComponent<RectTransform>();
        rtt.rect.Set( rtf.rect.x, rtf.rect.y, rtf.rect.width, rtf.rect.height);
        rtt.rect.size.Set( rtf.rect.size.x, rtf.rect.size.y) ;
        //to.transform.position = from.transform.position;
        //to.transform.rotation = from.transform.rotation;
        //to.transform.localScale = from.transform.localScale;
        //to.transform.localPosition = from.transform.localPosition;

    }
    public static T GetCopyOf<T>( this Component comp, T other ) where T : Component {
        Type type = comp.GetType();
        if ( type != other.GetType() ) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach ( var pinfo in pinfos ) {
            if ( pinfo.CanWrite ) {
                try {
                    pinfo.SetValue( comp, pinfo.GetValue( other, null ), null );
                } catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach ( var finfo in finfos ) {
            finfo.SetValue( comp, finfo.GetValue( other ) );
        }
        return comp as T;
    }


}
