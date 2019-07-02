#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class LunaMessageWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LunaMessage);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 10, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearAllHandlers", _m_ClearAllHandlers_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddMsgHandler", _m_AddMsgHandler_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMsgHandler", _m_GetMsgHandler_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMsgBody", _m_GetMsgBody_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMsgHeader", _m_GetMsgHeader_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMsgId", _m_GetMsgId_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMsgLength", _m_GetMsgLength_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MsgHeaderLength", LunaMessage.MsgHeaderLength);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MsgLength", LunaMessage.MsgLength);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					LunaMessage gen_ret = new LunaMessage();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LunaMessage constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearAllHandlers_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LunaMessage.ClearAllHandlers(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddMsgHandler_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _msgid = LuaAPI.xlua_tointeger(L, 1);
                    Response _handler = translator.GetDelegate<Response>(L, 2);
                    
                    LunaMessage.AddMsgHandler( _msgid, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMsgHandler_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _msgid = LuaAPI.xlua_tointeger(L, 1);
                    
                        Response gen_ret = LunaMessage.GetMsgHandler( _msgid );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMsgBody_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] _msg = LuaAPI.lua_tobytes(L, 1);
                    int _msgLength = LuaAPI.xlua_tointeger(L, 2);
                    
                        byte[] gen_ret = LunaMessage.GetMsgBody( _msg, _msgLength );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMsgHeader_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] _msg = LuaAPI.lua_tobytes(L, 1);
                    
                        byte[] gen_ret = LunaMessage.GetMsgHeader( _msg );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMsgId_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] _byteHeader = LuaAPI.lua_tobytes(L, 1);
                    
                        ushort gen_ret = LunaMessage.GetMsgId( _byteHeader );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMsgLength_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] _byteHeader = LuaAPI.lua_tobytes(L, 1);
                    
                        ushort gen_ret = LunaMessage.GetMsgLength( _byteHeader );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
