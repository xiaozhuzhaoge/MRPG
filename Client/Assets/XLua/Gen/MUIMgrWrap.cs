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
    public class MUIMgrWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(MUIMgr);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 1, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PushUI", _m_PushUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PopUI", _m_PopUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUI", _m_AddUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUI", _m_OpenUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUI", _m_CloseUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshUI", _m_RefreshUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveUI", _m_RemoveUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowAlert", _m_ShowAlert);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dispose", _m_Dispose);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Canvas", _g_get_Canvas);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					MUIMgr gen_ret = new MUIMgr();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MUIMgr constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PushUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    MUIBase _ba = (MUIBase)translator.GetObject(L, 2, typeof(MUIBase));
                    
                    gen_to_be_invoked.PushUI( _ba );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PopUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.PopUI(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    MUIBase _ba = (MUIBase)translator.GetObject(L, 2, typeof(MUIBase));
                    
                    gen_to_be_invoked.AddUI( _ba );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _UiName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.OpenUI( _UiName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _UiName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.CloseUI( _UiName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _UiName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.RefreshUI( _UiName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _UiName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.RemoveUI( _UiName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowAlert(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 8&& translator.Assignable<OpenType>(L, 2)&& translator.Assignable<System.Action>(L, 3)&& translator.Assignable<System.Action>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 6) || LuaAPI.lua_type(L, 6) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 8) || LuaAPI.lua_type(L, 8) == LuaTypes.LUA_TSTRING)) 
                {
                    OpenType _type;translator.Get(L, 2, out _type);
                    System.Action _one = translator.GetDelegate<System.Action>(L, 3);
                    System.Action _two = translator.GetDelegate<System.Action>(L, 4);
                    string _title = LuaAPI.lua_tostring(L, 5);
                    string _content = LuaAPI.lua_tostring(L, 6);
                    string _oneText = LuaAPI.lua_tostring(L, 7);
                    string _twoText = LuaAPI.lua_tostring(L, 8);
                    
                    gen_to_be_invoked.ShowAlert( _type, _one, _two, _title, _content, _oneText, _twoText );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& translator.Assignable<OpenType>(L, 2)&& translator.Assignable<System.Action>(L, 3)&& translator.Assignable<System.Action>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 6) || LuaAPI.lua_type(L, 6) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING)) 
                {
                    OpenType _type;translator.Get(L, 2, out _type);
                    System.Action _one = translator.GetDelegate<System.Action>(L, 3);
                    System.Action _two = translator.GetDelegate<System.Action>(L, 4);
                    string _title = LuaAPI.lua_tostring(L, 5);
                    string _content = LuaAPI.lua_tostring(L, 6);
                    string _oneText = LuaAPI.lua_tostring(L, 7);
                    
                    gen_to_be_invoked.ShowAlert( _type, _one, _two, _title, _content, _oneText );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& translator.Assignable<OpenType>(L, 2)&& translator.Assignable<System.Action>(L, 3)&& translator.Assignable<System.Action>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 6) || LuaAPI.lua_type(L, 6) == LuaTypes.LUA_TSTRING)) 
                {
                    OpenType _type;translator.Get(L, 2, out _type);
                    System.Action _one = translator.GetDelegate<System.Action>(L, 3);
                    System.Action _two = translator.GetDelegate<System.Action>(L, 4);
                    string _title = LuaAPI.lua_tostring(L, 5);
                    string _content = LuaAPI.lua_tostring(L, 6);
                    
                    gen_to_be_invoked.ShowAlert( _type, _one, _two, _title, _content );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<OpenType>(L, 2)&& translator.Assignable<System.Action>(L, 3)&& translator.Assignable<System.Action>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    OpenType _type;translator.Get(L, 2, out _type);
                    System.Action _one = translator.GetDelegate<System.Action>(L, 3);
                    System.Action _two = translator.GetDelegate<System.Action>(L, 4);
                    string _title = LuaAPI.lua_tostring(L, 5);
                    
                    gen_to_be_invoked.ShowAlert( _type, _one, _two, _title );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<OpenType>(L, 2)&& translator.Assignable<System.Action>(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    OpenType _type;translator.Get(L, 2, out _type);
                    System.Action _one = translator.GetDelegate<System.Action>(L, 3);
                    System.Action _two = translator.GetDelegate<System.Action>(L, 4);
                    
                    gen_to_be_invoked.ShowAlert( _type, _one, _two );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<OpenType>(L, 2)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    OpenType _type;translator.Get(L, 2, out _type);
                    System.Action _one = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.ShowAlert( _type, _one );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to MUIMgr.ShowAlert!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Dispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Canvas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MUIMgr gen_to_be_invoked = (MUIMgr)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Canvas);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
