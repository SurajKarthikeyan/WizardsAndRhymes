﻿//////////////////////////////////////////////////////
// MK Glow URP Renderer Feature	    		        //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2021 All rights reserved.            //
//////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
#if UNITY_2023_3_OR_NEWER
using UnityEngine.Rendering.RenderGraphModule;
#endif

namespace MK.Glow.URP
{
    public class MKGlowRendererFeature : ScriptableRendererFeature, MK.Glow.ISettings
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        // Renderer Feature Only Properties
        /////////////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR
        public bool showEditorMainBehavior = true;
		public bool showEditorBloomBehavior;
		public bool showEditorLensSurfaceBehavior;
		public bool showEditorLensFlareBehavior;
		public bool showEditorGlareBehavior;
        #endif

        public enum Workmode
        {
            [UnityEngine.InspectorName("Post-Process Volumes")]
            PostProcessVolumes = 0,
            [UnityEngine.InspectorName("Global")]
            Global = 1
        };
        public Workmode workmode = Workmode.PostProcessVolumes;

        //Main
        public bool allowGeometryShaders = true;
        public bool allowComputeShaders = true;
        public RenderPriority renderPriority = RenderPriority.Balanced;
        public DebugView debugView = MK.Glow.DebugView.None;
        public Quality quality = MK.Glow.Quality.High;
        public AntiFlickerMode antiFlickerMode = AntiFlickerMode.Balanced;
        public Workflow workflow = MK.Glow.Workflow.Threshold;
        public LayerMask selectiveRenderLayerMask = -1;
        [Range(-1f, 1f)]
        public float anamorphicRatio = 0f;
        [Range(0f, 1f)]
        public float lumaScale = 0.5f;
        [Range(0f, 1f)]
		public float blooming = 0f;

        //Bloom
        [MK.Glow.MinMaxRange(0, 10)]
        public MinMaxRange bloomThreshold = new MinMaxRange(1.25f, 10f);
        [Range(1f, 10f)]
		public float bloomScattering = 7f;
		public float bloomIntensity = 1f;

        //LensSurface
        public bool allowLensSurface = false;
		public Texture2D lensSurfaceDirtTexture;
		public float lensSurfaceDirtIntensity = 2.5f;
		public Texture2D lensSurfaceDiffractionTexture;
		public float lensSurfaceDiffractionIntensity = 2.0f;

        //LensFlare
        public bool allowLensFlare = false;
        public LensFlareStyle lensFlareStyle = LensFlareStyle.Average;
        [Range(0f, 25f)]
		public float lensFlareGhostFade = 10.0f;
		public float lensFlareGhostIntensity = 1.0f;
        [MK.Glow.MinMaxRange(0, 10)]
		public MinMaxRange lensFlareThreshold = new MinMaxRange(1.3f, 10f);
        [Range(0f, 8f)]
		public float lensFlareScattering = 5f;
		public Texture2D lensFlareColorRamp;
        [Range(-100f, 100f)]
		public float lensFlareChromaticAberration = 53f;
        [Range(1, 4)]
		public int lensFlareGhostCount = 3;
        [Range(-1f, 1f)]
		public float lensFlareGhostDispersal = 0.6f;
        [Range(0f, 25f)]
		public float lensFlareHaloFade = 2f;
		public float lensFlareHaloIntensity = 1.0f;
        [Range(0f, 1f)]
		public float lensFlareHaloSize = 0.4f;

        //Glare
        public bool allowGlare = false;
        [Range(0.0f, 1.0f)]
        public float glareBlend = 0.33f;
        public float glareIntensity = 1f;
        [Range(0.0f, 360.0f)]
        public float glareAngle = 0f;
        [MK.Glow.MinMaxRange(0, 10)]
        public MinMaxRange glareThreshold = new MinMaxRange(1.25f, 10f);
        [Range(1, 4)]
        public int glareStreaks = 4;
        public GlareStyle glareStyle = GlareStyle.DistortedCross;
        [Range(0.0f, 4.0f)]
        public float glareScattering = 2f;
        //Sample0
        [Range(0f, 10f)]
        public float glareSample0Scattering = 5f;
        [Range(0f, 360f)]
        public float glareSample0Angle = 0f;
        public float glareSample0Intensity = 1f;
        [Range(-5f, 5f)]
        public float glareSample0Offset = 0f;
        //Sample1
        [Range(0f, 10f)]
        public float glareSample1Scattering = 5f;
        [Range(0f, 360f)]
        public float glareSample1Angle = 45f;
        public float glareSample1Intensity = 1f;
        [Range(-5f, 5f)]
        public float glareSample1Offset = 0f;
        //Sample0
        [Range(0f, 10f)]
        public float glareSample2Scattering = 5f;
        [Range(0f, 360f)]
        public float glareSample2Angle = 90f;
        public float glareSample2Intensity = 1f;
        [Range(-5f, 5f)]
        public float glareSample2Offset = 0f;
        //Sample0
        [Range(0f, 10f)]
        public float glareSample3Scattering = 5f;
        [Range(0f, 360f)]
        public float glareSample3Angle = 135f;
        public float glareSample3Intensity = 1f;
        [Range(-5f, 5f)]
        public float glareSample3Offset = 0f;

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Settings
        /////////////////////////////////////////////////////////////////////////////////////////////
        public bool GetAllowGeometryShaders()
        { 
            return false;
        }
        public bool GetAllowComputeShaders()
        { 
            return false;
        }
        public RenderPriority GetRenderPriority()
        { 
            return renderPriority;
        }
        public MK.Glow.DebugView GetDebugView()
        { 
			return debugView;
		}
        public MK.Glow.Quality GetQuality()
        { 
			return quality;
		}
        public MK.Glow.AntiFlickerMode GetAntiFlickerMode()
        { 
			return antiFlickerMode;
		}
        public MK.Glow.Workflow GetWorkflow()
        { 
			return workflow;
		}
        public LayerMask GetSelectiveRenderLayerMask()
        { 
			return selectiveRenderLayerMask;
		}
        public float GetAnamorphicRatio()
        { 
			return anamorphicRatio;
		}
        public float GetLumaScale()
        { 
			return lumaScale;
		}
		public float GetBlooming()
		{ 
			return blooming;
		}

        //Bloom
		public MK.Glow.MinMaxRange GetBloomThreshold()
		{ 
			return bloomThreshold;
		}
		public float GetBloomScattering()
		{ 
			return bloomScattering;
		}
		public float GetBloomIntensity()
		{ 
			return bloomIntensity;
		}

        //LensSurface
		public bool GetAllowLensSurface()
		{ 
			return allowLensSurface;
		}
		public Texture2D GetLensSurfaceDirtTexture()
		{ 
			return lensSurfaceDirtTexture;
		}
		public float GetLensSurfaceDirtIntensity()
		{ 
			return lensSurfaceDirtIntensity;
		}
		public Texture2D GetLensSurfaceDiffractionTexture()
		{ 
			return lensSurfaceDiffractionTexture;
		}
		public float GetLensSurfaceDiffractionIntensity()
		{ 
			return lensSurfaceDiffractionIntensity;
		}

        //LensFlare
		public bool GetAllowLensFlare()
		{ 
			return allowLensFlare;
		}
        public LensFlareStyle GetLensFlareStyle()
		{ 
			return lensFlareStyle;
		}
		public float GetLensFlareGhostFade()
		{ 
			return lensFlareGhostFade;
		}
		public float GetLensFlareGhostIntensity()
		{ 
			return lensFlareGhostIntensity;
		}
		public MK.Glow.MinMaxRange GetLensFlareThreshold()
		{ 
			return lensFlareThreshold;
		}
		public float GetLensFlareScattering()
		{ 
			return lensFlareScattering;
		}
		public Texture2D GetLensFlareColorRamp()
		{ 
			return lensFlareColorRamp;
		}
		public float GetLensFlareChromaticAberration()
		{ 
			return lensFlareChromaticAberration;
		}
		public int GetLensFlareGhostCount()
		{ 
			return lensFlareGhostCount;
		}
		public float GetLensFlareGhostDispersal()
		{ 
			return lensFlareGhostDispersal;
		}
		public float GetLensFlareHaloFade()
		{
			return lensFlareHaloFade;
		}
		public float GetLensFlareHaloIntensity()
		{ 
			return lensFlareHaloIntensity;
		}
		public float GetLensFlareHaloSize()
		{ 
			return lensFlareHaloSize;
		}

        public void SetLensFlareGhostFade(float fade)
        {
            lensFlareGhostFade = fade;
        }
        public void SetLensFlareGhostCount(int count)
        {
            lensFlareGhostCount = count;
        }
        public void SetLensFlareGhostDispersal(float dispersal)
        {
            lensFlareGhostDispersal = dispersal;
        }
        public void SetLensFlareHaloFade(float fade)
        {
            lensFlareHaloFade = fade;
        }
        public void SetLensFlareHaloSize(float size)
        {
            lensFlareHaloSize = size;
        }

        //Glare
		public bool GetAllowGlare()
		{ 
			return allowGlare;
		}
        public float GetGlareBlend()
        { 
			return glareBlend;
		}
        public float GetGlareIntensity()
        {
            return glareIntensity;
        }
        public float GetGlareAngle()
        {
            return glareAngle;
        }
		public MK.Glow.MinMaxRange GetGlareThreshold()
		{ 
			return glareThreshold;
		}
		public int GetGlareStreaks()
		{ 
			return glareStreaks;
		}
        public void SetGlareStreaks(int count)
        {
            glareStreaks = count;
        }
        public float GetGlareScattering()
        {
            return glareScattering;
        }
        public GlareStyle GetGlareStyle()
        {
            return glareStyle;
        }

        //Sample0
        public float GetGlareSample0Scattering()
        {
            return glareSample0Scattering;
        }
        public float GetGlareSample0Angle()
        {
            return glareSample0Angle;
        }
        public float GetGlareSample0Intensity()
        {
            return glareSample0Intensity;
        }
        public float GetGlareSample0Offset()
        {
            return glareSample0Offset;
        }

        public void SetGlareSample0Scattering(float scattering)
        {
            glareSample0Scattering = scattering;
        }
        public void SetGlareSample0Angle(float angle)
        {
            glareSample0Angle = angle;
        }
        public void SetGlareSample0Intensity(float intensity)
        {
            glareSample0Intensity = intensity;
        }
        public void SetGlareSample0Offset(float offset)
        {
            glareSample0Offset = offset;
        }

        //Sample1
        public float GetGlareSample1Scattering()
        {
            return glareSample1Scattering;
        }
        public float GetGlareSample1Angle()
        {
            return glareSample1Angle;
        }
        public float GetGlareSample1Intensity()
        {
            return glareSample1Intensity;
        }
        public float GetGlareSample1Offset()
        {
            return glareSample1Offset;
        }

        public void SetGlareSample1Scattering(float scattering)
        {
            glareSample1Scattering = scattering;
        }
        public void SetGlareSample1Angle(float angle)
        {
            glareSample1Angle = angle;
        }
        public void SetGlareSample1Intensity(float intensity)
        {
            glareSample1Intensity = intensity;
        }
        public void SetGlareSample1Offset(float offset)
        {
            glareSample1Offset = offset;
        }

        //Sample2
        public float GetGlareSample2Scattering()
        {
            return glareSample2Scattering;
        }
        public float GetGlareSample2Angle()
        {
            return glareSample2Angle;
        }
        public float GetGlareSample2Intensity()
        {
            return glareSample2Intensity;
        }
        public float GetGlareSample2Offset()
        {
            return glareSample2Offset;
        }

        public void SetGlareSample2Scattering(float scattering)
        {
            glareSample2Scattering = scattering;
        }
        public void SetGlareSample2Angle(float angle)
        {
            glareSample2Angle = angle;
        }
        public void SetGlareSample2Intensity(float intensity)
        {
            glareSample2Intensity = intensity;
        }
        public void SetGlareSample2Offset(float offset)
        {
            glareSample2Offset = offset;
        }

        //Sample3
        public float GetGlareSample3Scattering()
        {
            return glareSample3Scattering;
        }
        public float GetGlareSample3Angle()
        {
            return glareSample3Angle;
        }
        public float GetGlareSample3Intensity()
        {
            return glareSample3Intensity;
        }
        public float GetGlareSample3Offset()
        {
            return glareSample3Offset;
        }

        public void SetGlareSample3Scattering(float scattering)
        {
            glareSample3Scattering = scattering;
        }
        public void SetGlareSample3Angle(float angle)
        {
            glareSample3Angle = angle;
        }
        public void SetGlareSample3Intensity(float intensity)
        {
            glareSample3Intensity = intensity;
        }
        public void SetGlareSample3Offset(float offset)
        {
            glareSample3Offset = offset;
        }

        class MKGlowRenderPass : ScriptableRenderPass, MK.Glow.ICameraData
        {
            private MK.Glow.URP.MKGlow _mKGlowVolumeComponent = null;
            private MK.Glow.URP.MKGlow mKGlowVolumeComponent
            {
                get
                {
                    _mKGlowVolumeComponent = VolumeManager.instance.stack.GetComponent<MK.Glow.URP.MKGlow>();
                    return _mKGlowVolumeComponent;
                }
            }

            internal Effect effect = new Effect();
            internal ScriptableRenderer scriptableRenderer;
            internal MK.Glow.ISettings settingsRendererFeature = null;
            internal Workmode workmode = Workmode.PostProcessVolumes;
            private RenderTarget sourceRenderTarget, destinationRenderTarget;
            private MK.Glow.ISettings _settingsPostProcessingVolume = null;
            private MK.Glow.ISettings _activeSettings = null;
            private RenderTextureDescriptor _sourceDescriptor;
            private readonly int _rendererBufferID = Shader.PropertyToID("_MKGlowScriptableRendererOutput");
            private readonly string _profilerName = "MKGlow";
            private Material _copyMaterial;
            private readonly Shader _copyShader = Shader.Find("Hidden/MK/Glow/URP/Copy");
            private readonly int _mainTexID = Shader.PropertyToID("_MainTex");

            public MKGlowRenderPass()
            {
                this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
                this._copyMaterial = new Material(_copyShader) { hideFlags = HideFlags.HideAndDontSave };
            }

            public void Dispose()
            {
                PipelineExtensions.Destroy(_copyMaterial);
            }

            #if UNITY_2023_3_OR_NEWER
            [System.Obsolete]
            #endif
            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                _sourceDescriptor = cameraTextureDescriptor;
            }

            #if UNITY_2021_3_OR_NEWER
            #if UNITY_2023_3_OR_NEWER
            [System.Obsolete]
            #endif
            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) 
            {
                base.OnCameraSetup(cmd, ref renderingData);
                #if UNITY_2022_1_OR_NEWER
                    sourceRenderTarget.renderTargetIdentifier = scriptableRenderer.cameraColorTargetHandle;
                #else
                    sourceRenderTarget.renderTargetIdentifier = scriptableRenderer.cameraColorTarget;
                #endif
            }
            #endif

            private static Mesh _fullScreenMesh;
            public static Mesh fullscreenMesh
            {
                get
                {
                    if (_fullScreenMesh != null)
                        return _fullScreenMesh;

                    float topV = 1.0f;
                    float bottomV = 0.0f;

                    _fullScreenMesh = new Mesh { name = "Fullscreen Quad" };
                    _fullScreenMesh.SetVertices(new List<Vector3>
                    {
                        new Vector3(-1.0f, -1.0f, 0.0f),
                        new Vector3(-1.0f,  1.0f, 0.0f),
                        new Vector3(1.0f, -1.0f, 0.0f),
                        new Vector3(1.0f,  1.0f, 0.0f)
                    });

                    _fullScreenMesh.SetUVs(0, new List<Vector2>
                    {
                        new Vector2(0.0f, bottomV),
                        new Vector2(0.0f, topV),
                        new Vector2(1.0f, bottomV),
                        new Vector2(1.0f, topV)
                    });

                    _fullScreenMesh.SetIndices(new[] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, false);
                    _fullScreenMesh.UploadMeshData(true);
                    return _fullScreenMesh;
                }
            }

            #if UNITY_2023_3_OR_NEWER
            [System.Obsolete]
            #endif
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if(workmode == Workmode.PostProcessVolumes)
                {
                    if(mKGlowVolumeComponent == null || renderingData.cameraData.camera == null)
                        return;
                    
                    if(!_mKGlowVolumeComponent.IsActive())
                        return;
                    }
                else
                {
                    if(renderingData.cameraData.camera == null)
                        return;
                }

                _settingsPostProcessingVolume = _mKGlowVolumeComponent;
                _activeSettings = workmode == Workmode.PostProcessVolumes ? _settingsPostProcessingVolume : settingsRendererFeature;

                CommandBuffer cmd = CommandBufferPool.Get(_profilerName);
                _passData.cameraWidth = renderingData.cameraData.cameraTargetDescriptor.width;
                _passData.cameraHeight = renderingData.cameraData.cameraTargetDescriptor.height;
                #if UNITY_2020_2_OR_NEWER
                    _passData.stereoEnabled = renderingData.cameraData.xrRendering;
                #else
                    _passData.stereoEnabled = renderingData.cameraData.isStereoEnabled;
                #endif
                _passData.aspect = renderingData.cameraData.camera.aspect;
                _passData.worldToCameraMatrix = renderingData.cameraData.camera.worldToCameraMatrix;
                _passData.hasTargetTexture = renderingData.cameraData.camera.targetTexture != null ? true : false;

                #if !UNITY_2021_3_OR_NEWER
                    sourceRenderTarget.renderTargetIdentifier = scriptableRenderer.cameraColorTarget;
                #endif

                destinationRenderTarget.identifier = _rendererBufferID;

                #if UNITY_2018_2_OR_NEWER
                destinationRenderTarget.renderTargetIdentifier = new RenderTargetIdentifier(destinationRenderTarget.identifier, 0, CubemapFace.Unknown, -1);
                #else
                destinationRenderTarget.renderTargetIdentifier = new RenderTargetIdentifier(destinationRenderTarget.identifier);
                #endif

                cmd.GetTemporaryRT(destinationRenderTarget.identifier, _sourceDescriptor, FilterMode.Bilinear);
                cmd.SetGlobalTexture(_mainTexID, sourceRenderTarget.renderTargetIdentifier);
                cmd.SetRenderTarget(destinationRenderTarget.renderTargetIdentifier, 0, CubemapFace.Unknown, -1);
                cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, _copyMaterial, 0, 0);
                effect.Build(destinationRenderTarget, sourceRenderTarget, _activeSettings, cmd, this, renderingData.cameraData.camera);
                cmd.ReleaseTemporaryRT(destinationRenderTarget.identifier);

                context.ExecuteCommandBuffer(cmd);

                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }

            private PassData _passData = new PassData();
            private class PassData
            {
                #if UNITY_2023_3_OR_NEWER
                internal TextureHandle src;
                internal TextureHandle dest;
                #endif
                internal Workmode workmode = Workmode.PostProcessVolumes;
                internal MK.Glow.URP.MKGlow mKGlowVolumeComponent = null;
                #if UNITY_2023_3_OR_NEWER
                public UniversalCameraData universalCameraData = null;
                #endif
                internal MK.Glow.ISettings settingsRendererFeature = null;
                internal MK.Glow.Effect effect;
                internal MK.Glow.URP.MKGlowRendererFeature.MKGlowRenderPass rendererFeaturePass;
                public int mainTexID;
                public Material copyMaterial;

                public int cameraWidth;
                public int cameraHeight;
                public bool stereoEnabled;
                public float aspect;
                public Matrix4x4 worldToCameraMatrix;
                public bool hasTargetTexture;
            }

            #if UNITY_2023_3_OR_NEWER
            static void ExecutePass(PassData data, UnsafeGraphContext context)
            {
                CommandBuffer unsafeCmd = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);
                
                if(data.workmode == Workmode.PostProcessVolumes)
                {
                    if(data.mKGlowVolumeComponent == null || data.universalCameraData.camera == null)
                        return;
                    
                    if(!data.mKGlowVolumeComponent.IsActive())
                        return;
                    }
                else
                {
                    if(data.universalCameraData.camera == null)
                        return;
                }

                MK.Glow.ISettings _activeSettings = data.workmode == Workmode.PostProcessVolumes ? data.mKGlowVolumeComponent : data.settingsRendererFeature;

                CommandBuffer cmd = unsafeCmd;
                RenderTarget sourceRenderTarget = new RenderTarget();
                RenderTarget destinationRenderTarget = new RenderTarget();
                sourceRenderTarget.renderTargetIdentifier = data.src;
                destinationRenderTarget.renderTargetIdentifier = data.dest;
                
                cmd.SetGlobalTexture(data.mainTexID, sourceRenderTarget.renderTargetIdentifier);
                cmd.SetRenderTarget(destinationRenderTarget.renderTargetIdentifier, 0, CubemapFace.Unknown, -1);
                cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, data.copyMaterial, 0, 0);
                data.effect.Build(destinationRenderTarget, sourceRenderTarget, _activeSettings, cmd, data.rendererFeaturePass, data.universalCameraData.camera);
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                string passName = "MK Glow Render Graph";
    
                using (var builder = renderGraph.AddUnsafePass<PassData>(passName, out var passData))
                {
                    UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                    UniversalCameraData universalCameraData = frameData.Get<UniversalCameraData>();

                    passData.workmode = this.workmode;
                    passData.mKGlowVolumeComponent = this.mKGlowVolumeComponent;
                    passData.universalCameraData = universalCameraData;
                    passData.effect = this.effect;
                    passData.rendererFeaturePass = this;
                    passData.mainTexID = _mainTexID;
                    passData.copyMaterial = _copyMaterial;

                    passData.cameraWidth = universalCameraData.cameraTargetDescriptor.width;
                    passData.cameraHeight = universalCameraData.cameraTargetDescriptor.height;
                    passData.stereoEnabled = universalCameraData.xrRendering;
                    passData.aspect = universalCameraData.camera.aspect;
                    passData.worldToCameraMatrix = universalCameraData.camera.worldToCameraMatrix;
                    passData.hasTargetTexture = universalCameraData.camera.targetTexture != null ? true : false;
                    passData.settingsRendererFeature = settingsRendererFeature;
    
                    passData.src = resourceData.activeColorTexture;
    
                    RenderTextureDescriptor desc = universalCameraData.cameraTargetDescriptor;
                    desc.depthBufferBits = 0;
    
                    TextureHandle destination = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "_MKGlowScriptableRendererRenderGraphOutput", false);
                    passData.dest = destination;
    
                    builder.UseTexture(passData.src);
                    builder.UseTexture(passData.dest);
    
                    builder.AllowPassCulling(false);

                    _passData = passData;
                    builder.SetRenderFunc((PassData data, UnsafeGraphContext context) => ExecutePass(data, context));
                }
            }
            #endif

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Camera Data
            /////////////////////////////////////////////////////////////////////////////////////////////
            public int GetCameraWidth()
            {
                return _passData.cameraWidth;
            }
            public int GetCameraHeight()
            {
                return _passData.cameraHeight;
            }
            public bool GetStereoEnabled()
            {
                return _passData.stereoEnabled;
            }
            public float GetAspect()
            {
                return _passData.aspect;
            }
            public Matrix4x4 GetWorldToCameraMatrix()
            {
                return _passData.worldToCameraMatrix;
            }
            public bool GetOverwriteDescriptor()
            {
                return false;
            }
            public UnityEngine.Rendering.TextureDimension GetOverwriteDimension()
            {
                return UnityEngine.Rendering.TextureDimension.Tex2D;
            }
            public int GetOverwriteVolumeDepth()
            {
                return 1;
            }
            public bool GetTargetTexture()
            {
                return _passData.hasTargetTexture;
            }
        }

        private MKGlowRenderPass _mkGlowRenderPass;
        private readonly string _componentName = "MKGlow";

        public override void Create()
        {
            _mkGlowRenderPass = new MKGlowRenderPass();
            _mkGlowRenderPass.effect.Enable(RenderPipeline.SRP);
            _mkGlowRenderPass.settingsRendererFeature = this;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            name = _componentName;
            _mkGlowRenderPass.workmode = workmode;
            //_mkGlowRenderPass.ConfigureInput(ScriptableRenderPassInput.Color);

            if(workmode == Workmode.Global)
            {
                _mkGlowRenderPass.scriptableRenderer = renderer;
                renderer.EnqueuePass(_mkGlowRenderPass);
            }
            else
            {
                if(renderingData.cameraData.postProcessEnabled)
                {
                    _mkGlowRenderPass.scriptableRenderer = renderer;
                    renderer.EnqueuePass(_mkGlowRenderPass);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            _mkGlowRenderPass?.effect.Disable();
            _mkGlowRenderPass?.Dispose();
            _mkGlowRenderPass = null;
        }
    }
}