// This file was generated. Do not modify.
using System;

namespace Mlang.Reflection.Spirv;

[Flags]
public enum ImageOperands : uint
{
    
    None = 0x0000U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Bias = 0x0001U,
    
    Lod = 0x0002U,
    
    Grad = 0x0004U,
    
    ConstOffset = 0x0008U,
    [DependsOn(Capabilities = new[] { Capability.ImageGatherExtended })]
    Offset = 0x0010U,
    [DependsOn(Capabilities = new[] { Capability.ImageGatherExtended })]
    ConstOffsets = 0x0020U,
    
    Sample = 0x0040U,
    [DependsOn(Capabilities = new[] { Capability.MinLod })]
    MinLod = 0x0080U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    MakeTexelAvailable = 0x0100U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    MakeTexelAvailableKHR = 0x0100U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    MakeTexelVisible = 0x0200U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    MakeTexelVisibleKHR = 0x0200U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    NonPrivateTexel = 0x0400U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    NonPrivateTexelKHR = 0x0400U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    VolatileTexel = 0x0800U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    VolatileTexelKHR = 0x0800U,
    [DependsOn(Version = "1.4")]
    SignExtend = 0x1000U,
    [DependsOn(Version = "1.4")]
    ZeroExtend = 0x2000U,
}
[Flags]
public enum FPFastMathMode : uint
{
    
    None = 0x0000U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    NotNaN = 0x0001U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    NotInf = 0x0002U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    NSZ = 0x0004U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    AllowRecip = 0x0008U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Fast = 0x0010U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPFastMathModeINTEL })]
    AllowContractFastINTEL = 0x10000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPFastMathModeINTEL })]
    AllowReassocINTEL = 0x20000U,
}
[Flags]
public enum SelectionControl : uint
{
    
    None = 0x0000U,
    
    Flatten = 0x0001U,
    
    DontFlatten = 0x0002U,
}
[Flags]
public enum LoopControl : uint
{
    
    None = 0x0000U,
    
    Unroll = 0x0001U,
    
    DontUnroll = 0x0002U,
    [DependsOn(Version = "1.1")]
    DependencyInfinite = 0x0004U,
    [DependsOn(Version = "1.1")]
    DependencyLength = 0x0008U,
    [DependsOn(Version = "1.4")]
    MinIterations = 0x0010U,
    [DependsOn(Version = "1.4")]
    MaxIterations = 0x0020U,
    [DependsOn(Version = "1.4")]
    IterationMultiple = 0x0040U,
    [DependsOn(Version = "1.4")]
    PeelCount = 0x0080U,
    [DependsOn(Version = "1.4")]
    PartialCount = 0x0100U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGALoopControlsINTEL }, Extensions = new[] { "SPV_INTEL_fpga_loop_controls" })]
    InitiationIntervalINTEL = 0x10000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGALoopControlsINTEL }, Extensions = new[] { "SPV_INTEL_fpga_loop_controls" })]
    MaxConcurrencyINTEL = 0x20000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGALoopControlsINTEL }, Extensions = new[] { "SPV_INTEL_fpga_loop_controls" })]
    DependencyArrayINTEL = 0x40000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGALoopControlsINTEL }, Extensions = new[] { "SPV_INTEL_fpga_loop_controls" })]
    PipelineEnableINTEL = 0x80000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGALoopControlsINTEL }, Extensions = new[] { "SPV_INTEL_fpga_loop_controls" })]
    LoopCoalesceINTEL = 0x100000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGALoopControlsINTEL }, Extensions = new[] { "SPV_INTEL_fpga_loop_controls" })]
    MaxInterleavingINTEL = 0x200000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGALoopControlsINTEL }, Extensions = new[] { "SPV_INTEL_fpga_loop_controls" })]
    SpeculatedIterationsINTEL = 0x400000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGALoopControlsINTEL }, Extensions = new[] { "SPV_INTEL_fpga_loop_controls" })]
    NoFusionINTEL = 0x800000U,
}
[Flags]
public enum FunctionControl : uint
{
    
    None = 0x0000U,
    
    Inline = 0x0001U,
    
    DontInline = 0x0002U,
    
    Pure = 0x0004U,
    
    Const = 0x0008U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.OptNoneINTEL })]
    OptNoneINTEL = 0x10000U,
}
[Flags]
public enum MemorySemantics : uint
{
    
    Relaxed = 0x0000U,
    
    None = 0x0000U,
    
    Acquire = 0x0002U,
    
    Release = 0x0004U,
    
    AcquireRelease = 0x0008U,
    
    SequentiallyConsistent = 0x0010U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    UniformMemory = 0x0040U,
    
    SubgroupMemory = 0x0080U,
    
    WorkgroupMemory = 0x0100U,
    
    CrossWorkgroupMemory = 0x0200U,
    [DependsOn(Capabilities = new[] { Capability.AtomicStorage })]
    AtomicCounterMemory = 0x0400U,
    
    ImageMemory = 0x0800U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    OutputMemory = 0x1000U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    OutputMemoryKHR = 0x1000U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    MakeAvailable = 0x2000U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    MakeAvailableKHR = 0x2000U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    MakeVisible = 0x4000U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    MakeVisibleKHR = 0x4000U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    Volatile = 0x8000U,
}
[Flags]
public enum MemoryAccess : uint
{
    
    None = 0x0000U,
    
    Volatile = 0x0001U,
    
    Aligned = 0x0002U,
    
    Nontemporal = 0x0004U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    MakePointerAvailable = 0x0008U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    MakePointerAvailableKHR = 0x0008U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    MakePointerVisible = 0x0010U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    MakePointerVisibleKHR = 0x0010U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    NonPrivatePointer = 0x0020U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    NonPrivatePointerKHR = 0x0020U,
}
[Flags]
public enum KernelProfilingInfo : uint
{
    
    None = 0x0000U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    CmdExecTime = 0x0001U,
}
[Flags]
public enum RayFlags : uint
{
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR })]
    NoneKHR = 0x0000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR })]
    OpaqueKHR = 0x0001U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR })]
    NoOpaqueKHR = 0x0002U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR })]
    TerminateOnFirstHitKHR = 0x0004U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR })]
    SkipClosestHitShaderKHR = 0x0008U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR })]
    CullBackFacingTrianglesKHR = 0x0010U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR })]
    CullFrontFacingTrianglesKHR = 0x0020U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR })]
    CullOpaqueKHR = 0x0040U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR })]
    CullNoOpaqueKHR = 0x0080U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTraversalPrimitiveCullingKHR })]
    SkipTrianglesKHR = 0x0100U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTraversalPrimitiveCullingKHR })]
    SkipAABBsKHR = 0x0200U,
}
[Flags]
public enum FragmentShadingRate : uint
{
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShadingRateKHR })]
    Vertical2Pixels = 0x0001U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShadingRateKHR })]
    Vertical4Pixels = 0x0002U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShadingRateKHR })]
    Horizontal2Pixels = 0x0004U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShadingRateKHR })]
    Horizontal4Pixels = 0x0008U,
}
public enum SourceLanguage : uint
{
    
    Unknown = 0U,
    
    ESSL = 1U,
    
    GLSL = 2U,
    
    OpenCL_C = 3U,
    
    OpenCL_CPP = 4U,
    
    HLSL = 5U,
    
    CPP_for_OpenCL = 6U,
}
public enum ExecutionModel : uint
{
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Vertex = 0U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    TessellationControl = 1U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    TessellationEvaluation = 2U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    Geometry = 3U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Fragment = 4U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    GLCompute = 5U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Kernel = 6U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV })]
    TaskNV = 5267U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV })]
    MeshNV = 5268U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    RayGenerationNV = 5313U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    RayGenerationKHR = 5313U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    IntersectionNV = 5314U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    IntersectionKHR = 5314U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    AnyHitNV = 5315U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    AnyHitKHR = 5315U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    ClosestHitNV = 5316U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    ClosestHitKHR = 5316U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    MissNV = 5317U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    MissKHR = 5317U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    CallableNV = 5318U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR })]
    CallableKHR = 5318U,
}
public enum AddressingModel : uint
{
    
    Logical = 0U,
    [DependsOn(Capabilities = new[] { Capability.Addresses })]
    Physical32 = 1U,
    [DependsOn(Capabilities = new[] { Capability.Addresses })]
    Physical64 = 2U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.PhysicalStorageBufferAddresses }, Extensions = new[] { "SPV_EXT_physical_storage_buffer", "SPV_KHR_physical_storage_buffer" })]
    PhysicalStorageBuffer64 = 5348U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.PhysicalStorageBufferAddresses }, Extensions = new[] { "SPV_EXT_physical_storage_buffer" })]
    PhysicalStorageBuffer64EXT = 5348U,
}
public enum MemoryModel : uint
{
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Simple = 0U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    GLSL450 = 1U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpenCL = 2U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    Vulkan = 3U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel }, Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    VulkanKHR = 3U,
}
public enum ExecutionMode : uint
{
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    Invocations = 0U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    SpacingEqual = 1U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    SpacingFractionalEven = 2U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    SpacingFractionalOdd = 3U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    VertexOrderCw = 4U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    VertexOrderCcw = 5U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    PixelCenterInteger = 6U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OriginUpperLeft = 7U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OriginLowerLeft = 8U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    EarlyFragmentTests = 9U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    PointMode = 10U,
    [DependsOn(Capabilities = new[] { Capability.TransformFeedback })]
    Xfb = 11U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    DepthReplacing = 12U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    DepthGreater = 14U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    DepthLess = 15U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    DepthUnchanged = 16U,
    
    LocalSize = 17U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    LocalSizeHint = 18U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    InputPoints = 19U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    InputLines = 20U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    InputLinesAdjacency = 21U,
    [DependsOn(Capabilities = new[] { Capability.Geometry, Capability.Tessellation })]
    Triangles = 22U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    InputTrianglesAdjacency = 23U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    Quads = 24U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    Isolines = 25U,
    [DependsOn(Capabilities = new[] { Capability.Geometry, Capability.Tessellation, Capability.MeshShadingNV })]
    OutputVertices = 26U,
    [DependsOn(Capabilities = new[] { Capability.Geometry, Capability.MeshShadingNV })]
    OutputPoints = 27U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    OutputLineStrip = 28U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    OutputTriangleStrip = 29U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    VecTypeHint = 30U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    ContractionOff = 31U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.Kernel })]
    Initializer = 33U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.Kernel })]
    Finalizer = 34U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.SubgroupDispatch })]
    SubgroupSize = 35U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.SubgroupDispatch })]
    SubgroupsPerWorkgroup = 36U,
    [DependsOn(Version = "1.2", Capabilities = new[] { Capability.SubgroupDispatch })]
    SubgroupsPerWorkgroupId = 37U,
    [DependsOn(Version = "1.2")]
    LocalSizeId = 38U,
    [DependsOn(Version = "1.2", Capabilities = new[] { Capability.Kernel })]
    LocalSizeHintId = 39U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_subgroup_uniform_control_flow" })]
    SubgroupUniformControlFlowKHR = 4421U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SampleMaskPostDepthCoverage }, Extensions = new[] { "SPV_KHR_post_depth_coverage" })]
    PostDepthCoverage = 4446U,
    [DependsOn(Version = "1.4", Capabilities = new[] { Capability.DenormPreserve }, Extensions = new[] { "SPV_KHR_float_controls" })]
    DenormPreserve = 4459U,
    [DependsOn(Version = "1.4", Capabilities = new[] { Capability.DenormFlushToZero }, Extensions = new[] { "SPV_KHR_float_controls" })]
    DenormFlushToZero = 4460U,
    [DependsOn(Version = "1.4", Capabilities = new[] { Capability.SignedZeroInfNanPreserve }, Extensions = new[] { "SPV_KHR_float_controls" })]
    SignedZeroInfNanPreserve = 4461U,
    [DependsOn(Version = "1.4", Capabilities = new[] { Capability.RoundingModeRTE }, Extensions = new[] { "SPV_KHR_float_controls" })]
    RoundingModeRTE = 4462U,
    [DependsOn(Version = "1.4", Capabilities = new[] { Capability.RoundingModeRTZ }, Extensions = new[] { "SPV_KHR_float_controls" })]
    RoundingModeRTZ = 4463U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.StencilExportEXT }, Extensions = new[] { "SPV_EXT_shader_stencil_export" })]
    StencilRefReplacingEXT = 5027U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    OutputLinesNV = 5269U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    OutputPrimitivesNV = 5270U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ComputeDerivativeGroupQuadsNV }, Extensions = new[] { "SPV_NV_compute_shader_derivatives" })]
    DerivativeGroupQuadsNV = 5289U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ComputeDerivativeGroupLinearNV }, Extensions = new[] { "SPV_NV_compute_shader_derivatives" })]
    DerivativeGroupLinearNV = 5290U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    OutputTrianglesNV = 5298U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShaderPixelInterlockEXT }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    PixelInterlockOrderedEXT = 5366U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShaderPixelInterlockEXT }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    PixelInterlockUnorderedEXT = 5367U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShaderSampleInterlockEXT }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    SampleInterlockOrderedEXT = 5368U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShaderSampleInterlockEXT }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    SampleInterlockUnorderedEXT = 5369U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShaderShadingRateInterlockEXT }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    ShadingRateInterlockOrderedEXT = 5370U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShaderShadingRateInterlockEXT }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    ShadingRateInterlockUnorderedEXT = 5371U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    SharedLocalMemorySizeINTEL = 5618U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RoundToInfinityINTEL })]
    RoundingModeRTPINTEL = 5620U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RoundToInfinityINTEL })]
    RoundingModeRTNINTEL = 5621U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RoundToInfinityINTEL })]
    FloatingPointModeALTINTEL = 5622U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RoundToInfinityINTEL })]
    FloatingPointModeIEEEINTEL = 5623U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.KernelAttributesINTEL }, Extensions = new[] { "SPV_INTEL_kernel_attributes" })]
    MaxWorkgroupSizeINTEL = 5893U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.KernelAttributesINTEL }, Extensions = new[] { "SPV_INTEL_kernel_attributes" })]
    MaxWorkDimINTEL = 5894U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.KernelAttributesINTEL }, Extensions = new[] { "SPV_INTEL_kernel_attributes" })]
    NoGlobalOffsetINTEL = 5895U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAKernelAttributesINTEL }, Extensions = new[] { "SPV_INTEL_kernel_attributes" })]
    NumSIMDWorkitemsINTEL = 5896U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAKernelAttributesINTEL })]
    SchedulerTargetFmaxMhzINTEL = 5903U,
}
public enum StorageClass : uint
{
    
    UniformConstant = 0U,
    
    Input = 1U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Uniform = 2U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Output = 3U,
    
    Workgroup = 4U,
    
    CrossWorkgroup = 5U,
    [DependsOn(Capabilities = new[] { Capability.Shader, Capability.VectorComputeINTEL })]
    Private = 6U,
    
    Function = 7U,
    [DependsOn(Capabilities = new[] { Capability.GenericPointer })]
    Generic = 8U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    PushConstant = 9U,
    [DependsOn(Capabilities = new[] { Capability.AtomicStorage })]
    AtomicCounter = 10U,
    
    Image = 11U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_storage_buffer_storage_class", "SPV_KHR_variable_pointers" })]
    StorageBuffer = 12U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    CallableDataNV = 5328U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    CallableDataKHR = 5328U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    IncomingCallableDataNV = 5329U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    IncomingCallableDataKHR = 5329U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    RayPayloadNV = 5338U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    RayPayloadKHR = 5338U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    HitAttributeNV = 5339U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    HitAttributeKHR = 5339U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    IncomingRayPayloadNV = 5342U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    IncomingRayPayloadKHR = 5342U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    ShaderRecordBufferNV = 5343U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    ShaderRecordBufferKHR = 5343U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.PhysicalStorageBufferAddresses }, Extensions = new[] { "SPV_EXT_physical_storage_buffer", "SPV_KHR_physical_storage_buffer" })]
    PhysicalStorageBuffer = 5349U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.PhysicalStorageBufferAddresses }, Extensions = new[] { "SPV_EXT_physical_storage_buffer" })]
    PhysicalStorageBufferEXT = 5349U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FunctionPointersINTEL }, Extensions = new[] { "SPV_INTEL_function_pointers" })]
    CodeSectionINTEL = 5605U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.USMStorageClassesINTEL }, Extensions = new[] { "SPV_INTEL_usm_storage_classes" })]
    DeviceOnlyINTEL = 5936U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.USMStorageClassesINTEL }, Extensions = new[] { "SPV_INTEL_usm_storage_classes" })]
    HostOnlyINTEL = 5937U,
}
public enum Dim : uint
{
    [DependsOn(Capabilities = new[] { Capability.Sampled1D, Capability.Image1D })]
    Dim1D = 0U,
    [DependsOn(Capabilities = new[] { Capability.Shader, Capability.Kernel, Capability.ImageMSArray })]
    Dim2D = 1U,
    
    Dim3D = 2U,
    [DependsOn(Capabilities = new[] { Capability.Shader, Capability.ImageCubeArray })]
    Cube = 3U,
    [DependsOn(Capabilities = new[] { Capability.SampledRect, Capability.ImageRect })]
    Rect = 4U,
    [DependsOn(Capabilities = new[] { Capability.SampledBuffer, Capability.ImageBuffer })]
    Buffer = 5U,
    [DependsOn(Capabilities = new[] { Capability.InputAttachment })]
    SubpassData = 6U,
}
public enum SamplerAddressingMode : uint
{
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    None = 0U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    ClampToEdge = 1U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Clamp = 2U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Repeat = 3U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    RepeatMirrored = 4U,
}
public enum SamplerFilterMode : uint
{
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Nearest = 0U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Linear = 1U,
}
public enum ImageFormat : uint
{
    
    Unknown = 0U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba32f = 1U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba16f = 2U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    R32f = 3U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba8 = 4U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba8Snorm = 5U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg32f = 6U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg16f = 7U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R11fG11fB10f = 8U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R16f = 9U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rgba16 = 10U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rgb10A2 = 11U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg16 = 12U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg8 = 13U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R16 = 14U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R8 = 15U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rgba16Snorm = 16U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg16Snorm = 17U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg8Snorm = 18U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R16Snorm = 19U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R8Snorm = 20U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba32i = 21U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba16i = 22U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba8i = 23U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    R32i = 24U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg32i = 25U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg16i = 26U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg8i = 27U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R16i = 28U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R8i = 29U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba32ui = 30U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba16ui = 31U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Rgba8ui = 32U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    R32ui = 33U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rgb10a2ui = 34U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg32ui = 35U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg16ui = 36U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    Rg8ui = 37U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R16ui = 38U,
    [DependsOn(Capabilities = new[] { Capability.StorageImageExtendedFormats })]
    R8ui = 39U,
    [DependsOn(Capabilities = new[] { Capability.Int64ImageEXT })]
    R64ui = 40U,
    [DependsOn(Capabilities = new[] { Capability.Int64ImageEXT })]
    R64i = 41U,
}
public enum ImageChannelOrder : uint
{
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    R = 0U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    A = 1U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    RG = 2U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    RA = 3U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    RGB = 4U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    RGBA = 5U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    BGRA = 6U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    ARGB = 7U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Intensity = 8U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Luminance = 9U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Rx = 10U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    RGx = 11U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    RGBx = 12U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Depth = 13U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    DepthStencil = 14U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    sRGB = 15U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    sRGBx = 16U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    sRGBA = 17U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    sBGRA = 18U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    ABGR = 19U,
}
public enum ImageChannelDataType : uint
{
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    SnormInt8 = 0U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    SnormInt16 = 1U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnormInt8 = 2U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnormInt16 = 3U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnormShort565 = 4U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnormShort555 = 5U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnormInt101010 = 6U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    SignedInt8 = 7U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    SignedInt16 = 8U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    SignedInt32 = 9U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnsignedInt8 = 10U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnsignedInt16 = 11U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnsignedInt32 = 12U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    HalfFloat = 13U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Float = 14U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnormInt24 = 15U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    UnormInt101010_2 = 16U,
}
public enum FPRoundingMode : uint
{
    
    RTE = 0U,
    
    RTZ = 1U,
    
    RTP = 2U,
    
    RTN = 3U,
}
public enum FPDenormMode : uint
{
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FunctionFloatControlINTEL })]
    Preserve = 0U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FunctionFloatControlINTEL })]
    FlushToZero = 1U,
}
public enum QuantizationModes : uint
{
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    TRN = 0U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    TRN_ZERO = 1U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    RND = 2U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    RND_ZERO = 3U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    RND_INF = 4U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    RND_MIN_INF = 5U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    RND_CONV = 6U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    RND_CONV_ODD = 7U,
}
public enum FPOperationMode : uint
{
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FunctionFloatControlINTEL })]
    IEEE = 0U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FunctionFloatControlINTEL })]
    ALT = 1U,
}
public enum OverflowModes : uint
{
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    WRAP = 0U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    SAT = 1U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    SAT_ZERO = 2U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ArbitraryPrecisionFixedPointINTEL })]
    SAT_SYM = 3U,
}
public enum LinkageType : uint
{
    [DependsOn(Capabilities = new[] { Capability.Linkage })]
    Export = 0U,
    [DependsOn(Capabilities = new[] { Capability.Linkage })]
    Import = 1U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Linkage }, Extensions = new[] { "SPV_KHR_linkonce_odr" })]
    LinkOnceODR = 2U,
}
public enum AccessQualifier : uint
{
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    ReadOnly = 0U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    WriteOnly = 1U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    ReadWrite = 2U,
}
public enum FunctionParameterAttribute : uint
{
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Zext = 0U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Sext = 1U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    ByVal = 2U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Sret = 3U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    NoAlias = 4U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    NoCapture = 5U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    NoWrite = 6U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    NoReadWrite = 7U,
}
public enum Decoration : uint
{
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    RelaxedPrecision = 0U,
    [DependsOn(Capabilities = new[] { Capability.Shader, Capability.Kernel })]
    SpecId = 1U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Block = 2U,
    [Obsolete("Last version for this enumerant was 1.3")]
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    BufferBlock = 3U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    RowMajor = 4U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    ColMajor = 5U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    ArrayStride = 6U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    MatrixStride = 7U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    GLSLShared = 8U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    GLSLPacked = 9U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    CPacked = 10U,
    
    BuiltIn = 11U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    NoPerspective = 13U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Flat = 14U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    Patch = 15U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Centroid = 16U,
    [DependsOn(Capabilities = new[] { Capability.SampleRateShading })]
    Sample = 17U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Invariant = 18U,
    
    Restrict = 19U,
    
    Aliased = 20U,
    
    Volatile = 21U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Constant = 22U,
    
    Coherent = 23U,
    
    NonWritable = 24U,
    
    NonReadable = 25U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Uniform = 26U,
    [DependsOn(Version = "1.4", Capabilities = new[] { Capability.Shader })]
    UniformId = 27U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    SaturatedConversion = 28U,
    [DependsOn(Capabilities = new[] { Capability.GeometryStreams })]
    Stream = 29U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Location = 30U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Component = 31U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Index = 32U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Binding = 33U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    DescriptorSet = 34U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Offset = 35U,
    [DependsOn(Capabilities = new[] { Capability.TransformFeedback })]
    XfbBuffer = 36U,
    [DependsOn(Capabilities = new[] { Capability.TransformFeedback })]
    XfbStride = 37U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    FuncParamAttr = 38U,
    
    FPRoundingMode = 39U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    FPFastMathMode = 40U,
    [DependsOn(Capabilities = new[] { Capability.Linkage })]
    LinkageAttributes = 41U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    NoContraction = 42U,
    [DependsOn(Capabilities = new[] { Capability.InputAttachment })]
    InputAttachmentIndex = 43U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Alignment = 44U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.Addresses })]
    MaxByteOffset = 45U,
    [DependsOn(Version = "1.2", Capabilities = new[] { Capability.Kernel })]
    AlignmentId = 46U,
    [DependsOn(Version = "1.2", Capabilities = new[] { Capability.Addresses })]
    MaxByteOffsetId = 47U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_KHR_no_integer_wrap_decoration" })]
    NoSignedWrap = 4469U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_KHR_no_integer_wrap_decoration" })]
    NoUnsignedWrap = 4470U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_AMD_shader_explicit_vertex_parameter" })]
    ExplicitInterpAMD = 4999U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SampleMaskOverrideCoverageNV }, Extensions = new[] { "SPV_NV_sample_mask_override_coverage" })]
    OverrideCoverageNV = 5248U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.GeometryShaderPassthroughNV }, Extensions = new[] { "SPV_NV_geometry_shader_passthrough" })]
    PassthroughNV = 5250U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderViewportMaskNV })]
    ViewportRelativeNV = 5252U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderStereoViewNV }, Extensions = new[] { "SPV_NV_stereo_view_rendering" })]
    SecondaryViewportRelativeNV = 5256U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    PerPrimitiveNV = 5271U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    PerViewNV = 5272U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    PerTaskNV = 5273U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentBarycentricNV }, Extensions = new[] { "SPV_NV_fragment_shader_barycentric" })]
    PerVertexNV = 5285U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform })]
    NonUniform = 5300U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    NonUniformEXT = 5300U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.PhysicalStorageBufferAddresses }, Extensions = new[] { "SPV_EXT_physical_storage_buffer", "SPV_KHR_physical_storage_buffer" })]
    RestrictPointer = 5355U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.PhysicalStorageBufferAddresses }, Extensions = new[] { "SPV_EXT_physical_storage_buffer" })]
    RestrictPointerEXT = 5355U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.PhysicalStorageBufferAddresses }, Extensions = new[] { "SPV_EXT_physical_storage_buffer", "SPV_KHR_physical_storage_buffer" })]
    AliasedPointer = 5356U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.PhysicalStorageBufferAddresses }, Extensions = new[] { "SPV_EXT_physical_storage_buffer" })]
    AliasedPointerEXT = 5356U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    SIMTCallINTEL = 5599U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IndirectReferencesINTEL }, Extensions = new[] { "SPV_INTEL_function_pointers" })]
    ReferencedIndirectlyINTEL = 5602U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.AsmINTEL })]
    ClobberINTEL = 5607U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.AsmINTEL })]
    SideEffectsINTEL = 5608U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    VectorComputeVariableINTEL = 5624U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    FuncParamIOKindINTEL = 5625U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    VectorComputeFunctionINTEL = 5626U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    StackCallINTEL = 5627U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    GlobalVariableOffsetINTEL = 5628U,
    [DependsOn(Version = "1.4")]
    CounterBuffer = 5634U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_GOOGLE_hlsl_functionality1" })]
    HlslCounterBufferGOOGLE = 5634U,
    [DependsOn(Version = "1.4")]
    UserSemantic = 5635U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_GOOGLE_hlsl_functionality1" })]
    HlslSemanticGOOGLE = 5635U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_GOOGLE_user_type" })]
    UserTypeGOOGLE = 5636U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FunctionFloatControlINTEL })]
    FunctionRoundingModeINTEL = 5822U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FunctionFloatControlINTEL })]
    FunctionDenormModeINTEL = 5823U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    RegisterINTEL = 5825U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    MemoryINTEL = 5826U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    NumbanksINTEL = 5827U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    BankwidthINTEL = 5828U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    MaxPrivateCopiesINTEL = 5829U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    SinglepumpINTEL = 5830U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    DoublepumpINTEL = 5831U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    MaxReplicatesINTEL = 5832U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    SimpleDualPortINTEL = 5833U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    MergeINTEL = 5834U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    BankBitsINTEL = 5835U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAttributesINTEL }, Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    ForcePow2DepthINTEL = 5836U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAccessesINTEL })]
    BurstCoalesceINTEL = 5899U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAccessesINTEL })]
    CacheSizeINTEL = 5900U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAccessesINTEL })]
    DontStaticallyCoalesceINTEL = 5901U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAMemoryAccessesINTEL })]
    PrefetchINTEL = 5902U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGAClusterAttributesINTEL })]
    StallEnableINTEL = 5905U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.LoopFuseINTEL })]
    FuseLoopsInFunctionINTEL = 5907U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGABufferLocationINTEL })]
    BufferLocationINTEL = 5921U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IOPipesINTEL })]
    IOPipeStorageINTEL = 5944U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FunctionFloatControlINTEL })]
    FunctionFloatingPointModeINTEL = 6080U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    SingleElementVectorINTEL = 6085U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    VectorComputeCallableFunctionINTEL = 6087U,
}
public enum BuiltIn : uint
{
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Position = 0U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    PointSize = 1U,
    [DependsOn(Capabilities = new[] { Capability.ClipDistance })]
    ClipDistance = 3U,
    [DependsOn(Capabilities = new[] { Capability.CullDistance })]
    CullDistance = 4U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    VertexId = 5U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    InstanceId = 6U,
    [DependsOn(Capabilities = new[] { Capability.Geometry, Capability.Tessellation, Capability.RayTracingNV, Capability.RayTracingKHR, Capability.MeshShadingNV })]
    PrimitiveId = 7U,
    [DependsOn(Capabilities = new[] { Capability.Geometry, Capability.Tessellation })]
    InvocationId = 8U,
    [DependsOn(Capabilities = new[] { Capability.Geometry, Capability.ShaderLayer, Capability.ShaderViewportIndexLayerEXT, Capability.MeshShadingNV })]
    Layer = 9U,
    [DependsOn(Capabilities = new[] { Capability.MultiViewport, Capability.ShaderViewportIndex, Capability.ShaderViewportIndexLayerEXT, Capability.MeshShadingNV })]
    ViewportIndex = 10U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    TessLevelOuter = 11U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    TessLevelInner = 12U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    TessCoord = 13U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    PatchVertices = 14U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    FragCoord = 15U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    PointCoord = 16U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    FrontFacing = 17U,
    [DependsOn(Capabilities = new[] { Capability.SampleRateShading })]
    SampleId = 18U,
    [DependsOn(Capabilities = new[] { Capability.SampleRateShading })]
    SamplePosition = 19U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    SampleMask = 20U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    FragDepth = 22U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    HelperInvocation = 23U,
    
    NumWorkgroups = 24U,
    
    WorkgroupSize = 25U,
    
    WorkgroupId = 26U,
    
    LocalInvocationId = 27U,
    
    GlobalInvocationId = 28U,
    
    LocalInvocationIndex = 29U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    WorkDim = 30U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    GlobalSize = 31U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    EnqueuedWorkgroupSize = 32U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    GlobalOffset = 33U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    GlobalLinearId = 34U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.GroupNonUniform, Capability.SubgroupBallotKHR })]
    SubgroupSize = 36U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    SubgroupMaxSize = 37U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.GroupNonUniform })]
    NumSubgroups = 38U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    NumEnqueuedSubgroups = 39U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.GroupNonUniform })]
    SubgroupId = 40U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.GroupNonUniform, Capability.SubgroupBallotKHR })]
    SubgroupLocalInvocationId = 41U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    VertexIndex = 42U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    InstanceIndex = 43U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot })]
    SubgroupEqMask = 4416U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot }, Extensions = new[] { "SPV_KHR_shader_ballot" })]
    SubgroupEqMaskKHR = 4416U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot })]
    SubgroupGeMask = 4417U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot }, Extensions = new[] { "SPV_KHR_shader_ballot" })]
    SubgroupGeMaskKHR = 4417U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot })]
    SubgroupGtMask = 4418U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot }, Extensions = new[] { "SPV_KHR_shader_ballot" })]
    SubgroupGtMaskKHR = 4418U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot })]
    SubgroupLeMask = 4419U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot }, Extensions = new[] { "SPV_KHR_shader_ballot" })]
    SubgroupLeMaskKHR = 4419U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot })]
    SubgroupLtMask = 4420U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.SubgroupBallotKHR, Capability.GroupNonUniformBallot }, Extensions = new[] { "SPV_KHR_shader_ballot" })]
    SubgroupLtMaskKHR = 4420U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.DrawParameters }, Extensions = new[] { "SPV_KHR_shader_draw_parameters" })]
    BaseVertex = 4424U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.DrawParameters }, Extensions = new[] { "SPV_KHR_shader_draw_parameters" })]
    BaseInstance = 4425U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.DrawParameters, Capability.MeshShadingNV }, Extensions = new[] { "SPV_KHR_shader_draw_parameters", "SPV_NV_mesh_shader" })]
    DrawIndex = 4426U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShadingRateKHR }, Extensions = new[] { "SPV_KHR_fragment_shading_rate" })]
    PrimitiveShadingRateKHR = 4432U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.DeviceGroup }, Extensions = new[] { "SPV_KHR_device_group" })]
    DeviceIndex = 4438U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.MultiView }, Extensions = new[] { "SPV_KHR_multiview" })]
    ViewIndex = 4440U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShadingRateKHR }, Extensions = new[] { "SPV_KHR_fragment_shading_rate" })]
    ShadingRateKHR = 4444U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_AMD_shader_explicit_vertex_parameter" })]
    BaryCoordNoPerspAMD = 4992U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_AMD_shader_explicit_vertex_parameter" })]
    BaryCoordNoPerspCentroidAMD = 4993U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_AMD_shader_explicit_vertex_parameter" })]
    BaryCoordNoPerspSampleAMD = 4994U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_AMD_shader_explicit_vertex_parameter" })]
    BaryCoordSmoothAMD = 4995U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_AMD_shader_explicit_vertex_parameter" })]
    BaryCoordSmoothCentroidAMD = 4996U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_AMD_shader_explicit_vertex_parameter" })]
    BaryCoordSmoothSampleAMD = 4997U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_AMD_shader_explicit_vertex_parameter" })]
    BaryCoordPullModelAMD = 4998U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.StencilExportEXT }, Extensions = new[] { "SPV_EXT_shader_stencil_export" })]
    FragStencilRefEXT = 5014U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderViewportMaskNV, Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_viewport_array2", "SPV_NV_mesh_shader" })]
    ViewportMaskNV = 5253U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderStereoViewNV }, Extensions = new[] { "SPV_NV_stereo_view_rendering" })]
    SecondaryPositionNV = 5257U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderStereoViewNV }, Extensions = new[] { "SPV_NV_stereo_view_rendering" })]
    SecondaryViewportMaskNV = 5258U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.PerViewAttributesNV, Capability.MeshShadingNV }, Extensions = new[] { "SPV_NVX_multiview_per_view_attributes", "SPV_NV_mesh_shader" })]
    PositionPerViewNV = 5261U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.PerViewAttributesNV, Capability.MeshShadingNV }, Extensions = new[] { "SPV_NVX_multiview_per_view_attributes", "SPV_NV_mesh_shader" })]
    ViewportMaskPerViewNV = 5262U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentFullyCoveredEXT }, Extensions = new[] { "SPV_EXT_fragment_fully_covered" })]
    FullyCoveredEXT = 5264U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    TaskCountNV = 5274U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    PrimitiveCountNV = 5275U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    PrimitiveIndicesNV = 5276U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    ClipDistancePerViewNV = 5277U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    CullDistancePerViewNV = 5278U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    LayerPerViewNV = 5279U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    MeshViewCountNV = 5280U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    MeshViewIndicesNV = 5281U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentBarycentricNV }, Extensions = new[] { "SPV_NV_fragment_shader_barycentric" })]
    BaryCoordNV = 5286U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentBarycentricNV }, Extensions = new[] { "SPV_NV_fragment_shader_barycentric" })]
    BaryCoordNoPerspNV = 5287U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentDensityEXT, Capability.ShadingRateNV }, Extensions = new[] { "SPV_EXT_fragment_invocation_density", "SPV_NV_shading_rate" })]
    FragSizeEXT = 5292U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShadingRateNV, Capability.FragmentDensityEXT }, Extensions = new[] { "SPV_NV_shading_rate", "SPV_EXT_fragment_invocation_density" })]
    FragmentSizeNV = 5292U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentDensityEXT, Capability.ShadingRateNV }, Extensions = new[] { "SPV_EXT_fragment_invocation_density", "SPV_NV_shading_rate" })]
    FragInvocationCountEXT = 5293U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShadingRateNV, Capability.FragmentDensityEXT }, Extensions = new[] { "SPV_NV_shading_rate", "SPV_EXT_fragment_invocation_density" })]
    InvocationsPerPixelNV = 5293U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    LaunchIdNV = 5319U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    LaunchIdKHR = 5319U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    LaunchSizeNV = 5320U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    LaunchSizeKHR = 5320U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    WorldRayOriginNV = 5321U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    WorldRayOriginKHR = 5321U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    WorldRayDirectionNV = 5322U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    WorldRayDirectionKHR = 5322U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    ObjectRayOriginNV = 5323U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    ObjectRayOriginKHR = 5323U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    ObjectRayDirectionNV = 5324U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    ObjectRayDirectionKHR = 5324U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    RayTminNV = 5325U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    RayTminKHR = 5325U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    RayTmaxNV = 5326U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    RayTmaxKHR = 5326U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    InstanceCustomIndexNV = 5327U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    InstanceCustomIndexKHR = 5327U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    ObjectToWorldNV = 5330U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    ObjectToWorldKHR = 5330U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    WorldToObjectNV = 5331U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    WorldToObjectKHR = 5331U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV }, Extensions = new[] { "SPV_NV_ray_tracing" })]
    HitTNV = 5332U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    HitKindNV = 5333U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    HitKindKHR = 5333U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingMotionBlurNV }, Extensions = new[] { "SPV_NV_ray_tracing_motion_blur" })]
    CurrentRayTimeNV = 5334U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    IncomingRayFlagsNV = 5351U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    IncomingRayFlagsKHR = 5351U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingKHR }, Extensions = new[] { "SPV_KHR_ray_tracing" })]
    RayGeometryIndexKHR = 5352U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderSMBuiltinsNV }, Extensions = new[] { "SPV_NV_shader_sm_builtins" })]
    WarpsPerSMNV = 5374U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderSMBuiltinsNV }, Extensions = new[] { "SPV_NV_shader_sm_builtins" })]
    SMCountNV = 5375U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderSMBuiltinsNV }, Extensions = new[] { "SPV_NV_shader_sm_builtins" })]
    WarpIDNV = 5376U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderSMBuiltinsNV }, Extensions = new[] { "SPV_NV_shader_sm_builtins" })]
    SMIDNV = 5377U,
}
public enum Scope : uint
{
    
    CrossDevice = 0U,
    
    Device = 1U,
    
    Workgroup = 2U,
    
    Subgroup = 3U,
    
    Invocation = 4U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    QueueFamily = 5U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.VulkanMemoryModel })]
    QueueFamilyKHR = 5U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingKHR })]
    ShaderCallKHR = 6U,
}
public enum GroupOperation : uint
{
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformBallot })]
    Reduce = 0U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformBallot })]
    InclusiveScan = 1U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformBallot })]
    ExclusiveScan = 2U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformClustered })]
    ClusteredReduce = 3U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.GroupNonUniformPartitionedNV }, Extensions = new[] { "SPV_NV_shader_subgroup_partitioned" })]
    PartitionedReduceNV = 6U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.GroupNonUniformPartitionedNV }, Extensions = new[] { "SPV_NV_shader_subgroup_partitioned" })]
    PartitionedInclusiveScanNV = 7U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.GroupNonUniformPartitionedNV }, Extensions = new[] { "SPV_NV_shader_subgroup_partitioned" })]
    PartitionedExclusiveScanNV = 8U,
}
public enum KernelEnqueueFlags : uint
{
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    NoWait = 0U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    WaitKernel = 1U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    WaitWorkGroup = 2U,
}
public enum Capability : uint
{
    
    Matrix = 0U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    Shader = 1U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Geometry = 2U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    Tessellation = 3U,
    
    Addresses = 4U,
    
    Linkage = 5U,
    
    Kernel = 6U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Vector16 = 7U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Float16Buffer = 8U,
    
    Float16 = 9U,
    
    Float64 = 10U,
    
    Int64 = 11U,
    [DependsOn(Capabilities = new[] { Capability.Int64 })]
    Int64Atomics = 12U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    ImageBasic = 13U,
    [DependsOn(Capabilities = new[] { Capability.ImageBasic })]
    ImageReadWrite = 14U,
    [DependsOn(Capabilities = new[] { Capability.ImageBasic })]
    ImageMipmap = 15U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    Pipes = 17U,
    [DependsOn(Extensions = new[] { "SPV_AMD_shader_ballot" })]
    Groups = 18U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    DeviceEnqueue = 19U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    LiteralSampler = 20U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    AtomicStorage = 21U,
    
    Int16 = 22U,
    [DependsOn(Capabilities = new[] { Capability.Tessellation })]
    TessellationPointSize = 23U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    GeometryPointSize = 24U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    ImageGatherExtended = 25U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    StorageImageMultisample = 27U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    UniformBufferArrayDynamicIndexing = 28U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    SampledImageArrayDynamicIndexing = 29U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    StorageBufferArrayDynamicIndexing = 30U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    StorageImageArrayDynamicIndexing = 31U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    ClipDistance = 32U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    CullDistance = 33U,
    [DependsOn(Capabilities = new[] { Capability.SampledCubeArray })]
    ImageCubeArray = 34U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    SampleRateShading = 35U,
    [DependsOn(Capabilities = new[] { Capability.SampledRect })]
    ImageRect = 36U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    SampledRect = 37U,
    [DependsOn(Capabilities = new[] { Capability.Addresses })]
    GenericPointer = 38U,
    
    Int8 = 39U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    InputAttachment = 40U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    SparseResidency = 41U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    MinLod = 42U,
    
    Sampled1D = 43U,
    [DependsOn(Capabilities = new[] { Capability.Sampled1D })]
    Image1D = 44U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    SampledCubeArray = 45U,
    
    SampledBuffer = 46U,
    [DependsOn(Capabilities = new[] { Capability.SampledBuffer })]
    ImageBuffer = 47U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    ImageMSArray = 48U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    StorageImageExtendedFormats = 49U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    ImageQuery = 50U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    DerivativeControl = 51U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    InterpolationFunction = 52U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    TransformFeedback = 53U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    GeometryStreams = 54U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    StorageImageReadWithoutFormat = 55U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    StorageImageWriteWithoutFormat = 56U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    MultiViewport = 57U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.DeviceEnqueue })]
    SubgroupDispatch = 58U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.Kernel })]
    NamedBarrier = 59U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.Pipes })]
    PipeStorage = 60U,
    [DependsOn(Version = "1.3")]
    GroupNonUniform = 61U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniform })]
    GroupNonUniformVote = 62U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniform })]
    GroupNonUniformArithmetic = 63U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniform })]
    GroupNonUniformBallot = 64U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniform })]
    GroupNonUniformShuffle = 65U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniform })]
    GroupNonUniformShuffleRelative = 66U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniform })]
    GroupNonUniformClustered = 67U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniform })]
    GroupNonUniformQuad = 68U,
    [DependsOn(Version = "1.5")]
    ShaderLayer = 69U,
    [DependsOn(Version = "1.5")]
    ShaderViewportIndex = 70U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_fragment_shading_rate" })]
    FragmentShadingRateKHR = 4422U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_shader_ballot" })]
    SubgroupBallotKHR = 4423U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_shader_draw_parameters" })]
    DrawParameters = 4427U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_workgroup_memory_explicit_layout" })]
    WorkgroupMemoryExplicitLayoutKHR = 4428U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.WorkgroupMemoryExplicitLayoutKHR }, Extensions = new[] { "SPV_KHR_workgroup_memory_explicit_layout" })]
    WorkgroupMemoryExplicitLayout8BitAccessKHR = 4429U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_workgroup_memory_explicit_layout" })]
    WorkgroupMemoryExplicitLayout16BitAccessKHR = 4430U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_subgroup_vote" })]
    SubgroupVoteKHR = 4431U,
    [DependsOn(Version = "1.3", Extensions = new[] { "SPV_KHR_16bit_storage" })]
    StorageBuffer16BitAccess = 4433U,
    [DependsOn(Version = "1.3", Extensions = new[] { "SPV_KHR_16bit_storage" })]
    StorageUniformBufferBlock16 = 4433U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.StorageBuffer16BitAccess, Capability.StorageUniformBufferBlock16 }, Extensions = new[] { "SPV_KHR_16bit_storage" })]
    UniformAndStorageBuffer16BitAccess = 4434U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.StorageBuffer16BitAccess, Capability.StorageUniformBufferBlock16 }, Extensions = new[] { "SPV_KHR_16bit_storage" })]
    StorageUniform16 = 4434U,
    [DependsOn(Version = "1.3", Extensions = new[] { "SPV_KHR_16bit_storage" })]
    StoragePushConstant16 = 4435U,
    [DependsOn(Version = "1.3", Extensions = new[] { "SPV_KHR_16bit_storage" })]
    StorageInputOutput16 = 4436U,
    [DependsOn(Version = "1.3", Extensions = new[] { "SPV_KHR_device_group" })]
    DeviceGroup = 4437U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_multiview" })]
    MultiView = 4439U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_variable_pointers" })]
    VariablePointersStorageBuffer = 4441U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.VariablePointersStorageBuffer }, Extensions = new[] { "SPV_KHR_variable_pointers" })]
    VariablePointers = 4442U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_shader_atomic_counter_ops" })]
    AtomicStorageOps = 4445U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_post_depth_coverage" })]
    SampleMaskPostDepthCoverage = 4447U,
    [DependsOn(Version = "1.5", Extensions = new[] { "SPV_KHR_8bit_storage" })]
    StorageBuffer8BitAccess = 4448U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.StorageBuffer8BitAccess }, Extensions = new[] { "SPV_KHR_8bit_storage" })]
    UniformAndStorageBuffer8BitAccess = 4449U,
    [DependsOn(Version = "1.5", Extensions = new[] { "SPV_KHR_8bit_storage" })]
    StoragePushConstant8 = 4450U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_KHR_float_controls" })]
    DenormPreserve = 4464U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_KHR_float_controls" })]
    DenormFlushToZero = 4465U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_KHR_float_controls" })]
    SignedZeroInfNanPreserve = 4466U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_KHR_float_controls" })]
    RoundingModeRTE = 4467U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_KHR_float_controls" })]
    RoundingModeRTZ = 4468U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_ray_query" })]
    RayQueryProvisionalKHR = 4471U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_ray_query" })]
    RayQueryKHR = 4472U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR, Capability.RayTracingKHR }, Extensions = new[] { "SPV_KHR_ray_query", "SPV_KHR_ray_tracing" })]
    RayTraversalPrimitiveCullingKHR = 4478U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_ray_tracing" })]
    RayTracingKHR = 4479U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_AMD_gpu_shader_half_float_fetch" })]
    Float16ImageAMD = 5008U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_AMD_texture_gather_bias_lod" })]
    ImageGatherBiasLodAMD = 5009U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_AMD_shader_fragment_mask" })]
    FragmentMaskAMD = 5010U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_shader_stencil_export" })]
    StencilExportEXT = 5013U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_AMD_shader_image_load_store_lod" })]
    ImageReadWriteLodAMD = 5015U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_shader_image_int64" })]
    Int64ImageEXT = 5016U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_shader_clock" })]
    ShaderClockKHR = 5055U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SampleRateShading }, Extensions = new[] { "SPV_NV_sample_mask_override_coverage" })]
    SampleMaskOverrideCoverageNV = 5249U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Geometry }, Extensions = new[] { "SPV_NV_geometry_shader_passthrough" })]
    GeometryShaderPassthroughNV = 5251U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MultiViewport }, Extensions = new[] { "SPV_EXT_shader_viewport_index_layer" })]
    ShaderViewportIndexLayerEXT = 5254U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MultiViewport }, Extensions = new[] { "SPV_NV_viewport_array2" })]
    ShaderViewportIndexLayerNV = 5254U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderViewportIndexLayerNV }, Extensions = new[] { "SPV_NV_viewport_array2" })]
    ShaderViewportMaskNV = 5255U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderViewportMaskNV }, Extensions = new[] { "SPV_NV_stereo_view_rendering" })]
    ShaderStereoViewNV = 5259U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MultiView }, Extensions = new[] { "SPV_NVX_multiview_per_view_attributes" })]
    PerViewAttributesNV = 5260U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_fragment_fully_covered" })]
    FragmentFullyCoveredEXT = 5265U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    MeshShadingNV = 5266U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_NV_shader_image_footprint" })]
    ImageFootprintNV = 5282U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_NV_fragment_shader_barycentric" })]
    FragmentBarycentricNV = 5284U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_NV_compute_shader_derivatives" })]
    ComputeDerivativeGroupQuadsNV = 5288U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_fragment_invocation_density", "SPV_NV_shading_rate" })]
    FragmentDensityEXT = 5291U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_NV_shading_rate", "SPV_EXT_fragment_invocation_density" })]
    ShadingRateNV = 5291U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_NV_shader_subgroup_partitioned" })]
    GroupNonUniformPartitionedNV = 5297U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.Shader })]
    ShaderNonUniform = 5301U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    ShaderNonUniformEXT = 5301U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.Shader })]
    RuntimeDescriptorArray = 5302U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    RuntimeDescriptorArrayEXT = 5302U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.InputAttachment })]
    InputAttachmentArrayDynamicIndexing = 5303U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.InputAttachment }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    InputAttachmentArrayDynamicIndexingEXT = 5303U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.SampledBuffer })]
    UniformTexelBufferArrayDynamicIndexing = 5304U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.SampledBuffer }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    UniformTexelBufferArrayDynamicIndexingEXT = 5304U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ImageBuffer })]
    StorageTexelBufferArrayDynamicIndexing = 5305U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ImageBuffer }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    StorageTexelBufferArrayDynamicIndexingEXT = 5305U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform })]
    UniformBufferArrayNonUniformIndexing = 5306U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    UniformBufferArrayNonUniformIndexingEXT = 5306U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform })]
    SampledImageArrayNonUniformIndexing = 5307U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    SampledImageArrayNonUniformIndexingEXT = 5307U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform })]
    StorageBufferArrayNonUniformIndexing = 5308U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    StorageBufferArrayNonUniformIndexingEXT = 5308U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform })]
    StorageImageArrayNonUniformIndexing = 5309U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ShaderNonUniform }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    StorageImageArrayNonUniformIndexingEXT = 5309U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.InputAttachment, Capability.ShaderNonUniform })]
    InputAttachmentArrayNonUniformIndexing = 5310U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.InputAttachment, Capability.ShaderNonUniform }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    InputAttachmentArrayNonUniformIndexingEXT = 5310U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.SampledBuffer, Capability.ShaderNonUniform })]
    UniformTexelBufferArrayNonUniformIndexing = 5311U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.SampledBuffer, Capability.ShaderNonUniform }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    UniformTexelBufferArrayNonUniformIndexingEXT = 5311U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ImageBuffer, Capability.ShaderNonUniform })]
    StorageTexelBufferArrayNonUniformIndexing = 5312U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.ImageBuffer, Capability.ShaderNonUniform }, Extensions = new[] { "SPV_EXT_descriptor_indexing" })]
    StorageTexelBufferArrayNonUniformIndexingEXT = 5312U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_NV_ray_tracing" })]
    RayTracingNV = 5340U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_NV_ray_tracing_motion_blur" })]
    RayTracingMotionBlurNV = 5341U,
    [DependsOn(Version = "1.5")]
    VulkanMemoryModel = 5345U,
    [DependsOn(Version = "1.5", Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    VulkanMemoryModelKHR = 5345U,
    [DependsOn(Version = "1.5")]
    VulkanMemoryModelDeviceScope = 5346U,
    [DependsOn(Version = "1.5", Extensions = new[] { "SPV_KHR_vulkan_memory_model" })]
    VulkanMemoryModelDeviceScopeKHR = 5346U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_physical_storage_buffer", "SPV_KHR_physical_storage_buffer" })]
    PhysicalStorageBufferAddresses = 5347U,
    [DependsOn(Version = "1.5", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_physical_storage_buffer" })]
    PhysicalStorageBufferAddressesEXT = 5347U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_NV_compute_shader_derivatives" })]
    ComputeDerivativeGroupLinearNV = 5350U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_ray_tracing" })]
    RayTracingProvisionalKHR = 5353U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_NV_cooperative_matrix" })]
    CooperativeMatrixNV = 5357U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    FragmentShaderSampleInterlockEXT = 5363U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    FragmentShaderShadingRateInterlockEXT = 5372U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_NV_shader_sm_builtins" })]
    ShaderSMBuiltinsNV = 5373U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    FragmentShaderPixelInterlockEXT = 5378U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_demote_to_helper_invocation" })]
    DemoteToHelperInvocationEXT = 5379U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_subgroups" })]
    SubgroupShuffleINTEL = 5568U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_subgroups" })]
    SubgroupBufferBlockIOINTEL = 5569U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_subgroups" })]
    SubgroupImageBlockIOINTEL = 5570U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_media_block_io" })]
    SubgroupImageMediaBlockIOINTEL = 5579U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_float_controls2" })]
    RoundToInfinityINTEL = 5582U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_float_controls2" })]
    FloatingPointModeINTEL = 5583U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_INTEL_shader_integer_functions2" })]
    IntegerFunctions2INTEL = 5584U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_function_pointers" })]
    FunctionPointersINTEL = 5603U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_function_pointers" })]
    IndirectReferencesINTEL = 5604U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_inline_assembly" })]
    AsmINTEL = 5606U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_EXT_shader_atomic_float_min_max" })]
    AtomicFloat32MinMaxEXT = 5612U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_EXT_shader_atomic_float_min_max" })]
    AtomicFloat64MinMaxEXT = 5613U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_EXT_shader_atomic_float_min_max" })]
    AtomicFloat16MinMaxEXT = 5616U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorAnyINTEL }, Extensions = new[] { "SPV_INTEL_vector_compute" })]
    VectorComputeINTEL = 5617U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_vector_compute" })]
    VectorAnyINTEL = 5619U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_expect_assume" })]
    ExpectAssumeKHR = 5629U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_device_side_avc_motion_estimation" })]
    SubgroupAvcMotionEstimationINTEL = 5696U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_device_side_avc_motion_estimation" })]
    SubgroupAvcMotionEstimationIntraINTEL = 5697U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_device_side_avc_motion_estimation" })]
    SubgroupAvcMotionEstimationChromaINTEL = 5698U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_variable_length_array" })]
    VariableLengthArrayINTEL = 5817U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_float_controls2" })]
    FunctionFloatControlINTEL = 5821U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_fpga_memory_attributes" })]
    FPGAMemoryAttributesINTEL = 5824U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Kernel }, Extensions = new[] { "SPV_INTEL_fp_fast_math_mode" })]
    FPFastMathModeINTEL = 5837U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_arbitrary_precision_integers" })]
    ArbitraryPrecisionIntegersINTEL = 5844U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_arbitrary_precision_floating_point" })]
    ArbitraryPrecisionFloatingPointINTEL = 5845U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_unstructured_loop_controls" })]
    UnstructuredLoopControlsINTEL = 5886U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_fpga_loop_controls" })]
    FPGALoopControlsINTEL = 5888U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_kernel_attributes" })]
    KernelAttributesINTEL = 5892U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_kernel_attributes" })]
    FPGAKernelAttributesINTEL = 5897U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_fpga_memory_accesses" })]
    FPGAMemoryAccessesINTEL = 5898U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_fpga_cluster_attributes" })]
    FPGAClusterAttributesINTEL = 5904U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_loop_fuse" })]
    LoopFuseINTEL = 5906U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_fpga_buffer_location" })]
    FPGABufferLocationINTEL = 5920U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_arbitrary_precision_fixed_point" })]
    ArbitraryPrecisionFixedPointINTEL = 5922U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_usm_storage_classes" })]
    USMStorageClassesINTEL = 5935U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_io_pipes" })]
    IOPipesINTEL = 5943U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_blocking_pipes" })]
    BlockingPipesINTEL = 5945U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_fpga_reg" })]
    FPGARegINTEL = 5948U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_integer_dot_product" })]
    DotProductInputAllKHR = 6016U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Int8 }, Extensions = new[] { "SPV_KHR_integer_dot_product" })]
    DotProductInput4x8BitKHR = 6017U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_integer_dot_product" })]
    DotProductInput4x8BitPackedKHR = 6018U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_integer_dot_product" })]
    DotProductKHR = 6019U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_bit_instructions" })]
    BitInstructions = 6025U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_shader_atomic_float_add" })]
    AtomicFloat32AddEXT = 6033U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_shader_atomic_float_add" })]
    AtomicFloat64AddEXT = 6034U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_long_constant_composite" })]
    LongConstantCompositeINTEL = 6089U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_optnone" })]
    OptNoneINTEL = 6094U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_EXT_shader_atomic_float16_add" })]
    AtomicFloat16AddEXT = 6095U,
    [DependsOn(Version = "None", Extensions = new[] { "SPV_INTEL_debug_module" })]
    DebugInfoModuleINTEL = 6114U,
}
public enum RayQueryIntersection : uint
{
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR })]
    RayQueryCandidateIntersectionKHR = 0U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR })]
    RayQueryCommittedIntersectionKHR = 1U,
}
public enum RayQueryCommittedIntersectionType : uint
{
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR })]
    RayQueryCommittedIntersectionNoneKHR = 0U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR })]
    RayQueryCommittedIntersectionTriangleKHR = 1U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR })]
    RayQueryCommittedIntersectionGeneratedKHR = 2U,
}
public enum RayQueryCandidateIntersectionType : uint
{
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR })]
    RayQueryCandidateIntersectionTriangleKHR = 0U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR })]
    RayQueryCandidateIntersectionAABBKHR = 1U,
}
public enum PackedVectorFormat : uint
{
    [DependsOn(Version = "None", Extensions = new[] { "SPV_KHR_integer_dot_product" })]
    PackedVectorFormat4x8BitKHR = 0U,
}

