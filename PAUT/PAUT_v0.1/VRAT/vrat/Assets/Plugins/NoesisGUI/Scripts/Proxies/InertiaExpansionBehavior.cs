/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.4
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

namespace Noesis
{

public class InertiaExpansionBehavior : IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal InertiaExpansionBehavior(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(InertiaExpansionBehavior obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~InertiaExpansionBehavior() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          if (Noesis.Extend.Initialized) { NoesisGUI_PINVOKE.delete_InertiaExpansionBehavior(swigCPtr);}
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public float DesiredDeceleration {
    get {
      return GetDesiredDecelerationHelper();
    }
    set {
      SetDesiredDecelerationHelper(value);
    }
  }

  private float GetDesiredDecelerationHelper() {
    float ret = NoesisGUI_PINVOKE.InertiaExpansionBehavior_GetDesiredDecelerationHelper(swigCPtr);
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
    return ret;
  }

  private void SetDesiredDecelerationHelper(float v) {
    NoesisGUI_PINVOKE.InertiaExpansionBehavior_SetDesiredDecelerationHelper(swigCPtr, v);
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
  }

  public InertiaExpansionBehavior() : this(NoesisGUI_PINVOKE.new_InertiaExpansionBehavior(), true) {
    #if UNITY_EDITOR || NOESIS_API
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    #endif
  }

}

}
