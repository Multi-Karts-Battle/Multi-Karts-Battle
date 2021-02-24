/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.2
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace RakNet {

using System;
using System.Runtime.InteropServices;

public class CommandParserInterface : IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal CommandParserInterface(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(CommandParserInterface obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~CommandParserInterface() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          RakNetPINVOKE.delete_CommandParserInterface(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public virtual string GetName() {
    string ret = RakNetPINVOKE.CommandParserInterface_GetName(swigCPtr);
    return ret;
  }

  public virtual void OnNewIncomingConnection(SystemAddress systemAddress, TransportInterface transport) {
    RakNetPINVOKE.CommandParserInterface_OnNewIncomingConnection(swigCPtr, SystemAddress.getCPtr(systemAddress), TransportInterface.getCPtr(transport));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void OnConnectionLost(SystemAddress systemAddress, TransportInterface transport) {
    RakNetPINVOKE.CommandParserInterface_OnConnectionLost(swigCPtr, SystemAddress.getCPtr(systemAddress), TransportInterface.getCPtr(transport));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void SendHelp(TransportInterface transport, SystemAddress systemAddress) {
    RakNetPINVOKE.CommandParserInterface_SendHelp(swigCPtr, TransportInterface.getCPtr(transport), SystemAddress.getCPtr(systemAddress));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual bool OnCommand(string command, uint numParameters, string[] parameterList, TransportInterface transport, SystemAddress systemAddress, string originalString) {
    bool ret = RakNetPINVOKE.CommandParserInterface_OnCommand(swigCPtr, command, numParameters, parameterList, TransportInterface.getCPtr(transport), SystemAddress.getCPtr(systemAddress), originalString);
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual void OnTransportChange(TransportInterface transport) {
    RakNetPINVOKE.CommandParserInterface_OnTransportChange(swigCPtr, TransportInterface.getCPtr(transport));
  }

  public virtual void SendCommandList(TransportInterface transport, SystemAddress systemAddress) {
    RakNetPINVOKE.CommandParserInterface_SendCommandList(swigCPtr, TransportInterface.getCPtr(transport), SystemAddress.getCPtr(systemAddress));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public static byte VARIABLE_NUMBER_OF_PARAMETERS {
    get {
      byte ret = RakNetPINVOKE.CommandParserInterface_VARIABLE_NUMBER_OF_PARAMETERS_get();
      return ret;
    } 
  }

  public virtual void RegisterCommand(byte parameterCount, string command, string commandHelp) {
    RakNetPINVOKE.CommandParserInterface_RegisterCommand(swigCPtr, parameterCount, command, commandHelp);
  }

  public virtual void ReturnResult(bool res, string command, TransportInterface transport, SystemAddress systemAddress) {
    RakNetPINVOKE.CommandParserInterface_ReturnResult__SWIG_0(swigCPtr, res, command, TransportInterface.getCPtr(transport), SystemAddress.getCPtr(systemAddress));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void ReturnResult(string res, string command, TransportInterface transport, SystemAddress systemAddress) {
    RakNetPINVOKE.CommandParserInterface_ReturnResult__SWIG_1(swigCPtr, res, command, TransportInterface.getCPtr(transport), SystemAddress.getCPtr(systemAddress));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void ReturnResult(SystemAddress res, string command, TransportInterface transport, SystemAddress systemAddress) {
    RakNetPINVOKE.CommandParserInterface_ReturnResult__SWIG_2(swigCPtr, SystemAddress.getCPtr(res), command, TransportInterface.getCPtr(transport), SystemAddress.getCPtr(systemAddress));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void ReturnResult(int res, string command, TransportInterface transport, SystemAddress systemAddress) {
    RakNetPINVOKE.CommandParserInterface_ReturnResult__SWIG_3(swigCPtr, res, command, TransportInterface.getCPtr(transport), SystemAddress.getCPtr(systemAddress));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void ReturnResult(string command, TransportInterface transport, SystemAddress systemAddress) {
    RakNetPINVOKE.CommandParserInterface_ReturnResult__SWIG_4(swigCPtr, command, TransportInterface.getCPtr(transport), SystemAddress.getCPtr(systemAddress));
    if (RakNetPINVOKE.SWIGPendingException.Pending) throw RakNetPINVOKE.SWIGPendingException.Retrieve();
  }

}

}
