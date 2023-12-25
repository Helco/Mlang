// This file was generated. Do not modify.
using System;

namespace Mlang.Reflection.Spirv;

public enum OpCode : uint
{
    OpNop = 0U,
    OpUndef = 1U,
    OpSourceContinued = 2U,
    OpSource = 3U,
    OpSourceExtension = 4U,
    OpName = 5U,
    OpMemberName = 6U,
    OpString = 7U,
    OpLine = 8U,
    OpExtension = 10U,
    OpExtInstImport = 11U,
    OpExtInst = 12U,
    OpMemoryModel = 14U,
    OpEntryPoint = 15U,
    OpExecutionMode = 16U,
    OpCapability = 17U,
    OpTypeVoid = 19U,
    OpTypeBool = 20U,
    OpTypeInt = 21U,
    OpTypeFloat = 22U,
    OpTypeVector = 23U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    OpTypeMatrix = 24U,
    OpTypeImage = 25U,
    OpTypeSampler = 26U,
    OpTypeSampledImage = 27U,
    OpTypeArray = 28U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpTypeRuntimeArray = 29U,
    OpTypeStruct = 30U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpTypeOpaque = 31U,
    OpTypePointer = 32U,
    OpTypeFunction = 33U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpTypeEvent = 34U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpTypeDeviceEvent = 35U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpTypeReserveId = 36U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpTypeQueue = 37U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpTypePipe = 38U,
    [DependsOn(Capabilities = new[] { Capability.Addresses, Capability.PhysicalStorageBufferAddresses })]
    OpTypeForwardPointer = 39U,
    OpConstantTrue = 41U,
    OpConstantFalse = 42U,
    OpConstant = 43U,
    OpConstantComposite = 44U,
    [DependsOn(Capabilities = new[] { Capability.LiteralSampler })]
    OpConstantSampler = 45U,
    OpConstantNull = 46U,
    OpSpecConstantTrue = 48U,
    OpSpecConstantFalse = 49U,
    OpSpecConstant = 50U,
    OpSpecConstantComposite = 51U,
    OpSpecConstantOp = 52U,
    OpFunction = 54U,
    OpFunctionParameter = 55U,
    OpFunctionEnd = 56U,
    OpFunctionCall = 57U,
    OpVariable = 59U,
    OpImageTexelPointer = 60U,
    OpLoad = 61U,
    OpStore = 62U,
    OpCopyMemory = 63U,
    [DependsOn(Capabilities = new[] { Capability.Addresses })]
    OpCopyMemorySized = 64U,
    OpAccessChain = 65U,
    OpInBoundsAccessChain = 66U,
    [DependsOn(Capabilities = new[] { Capability.Addresses, Capability.VariablePointers, Capability.VariablePointersStorageBuffer, Capability.PhysicalStorageBufferAddresses })]
    OpPtrAccessChain = 67U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpArrayLength = 68U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpGenericPtrMemSemantics = 69U,
    [DependsOn(Capabilities = new[] { Capability.Addresses })]
    OpInBoundsPtrAccessChain = 70U,
    OpDecorate = 71U,
    OpMemberDecorate = 72U,
    OpDecorationGroup = 73U,
    OpGroupDecorate = 74U,
    OpGroupMemberDecorate = 75U,
    OpVectorExtractDynamic = 77U,
    OpVectorInsertDynamic = 78U,
    OpVectorShuffle = 79U,
    OpCompositeConstruct = 80U,
    OpCompositeExtract = 81U,
    OpCompositeInsert = 82U,
    OpCopyObject = 83U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    OpTranspose = 84U,
    OpSampledImage = 86U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpImageSampleImplicitLod = 87U,
    OpImageSampleExplicitLod = 88U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpImageSampleDrefImplicitLod = 89U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpImageSampleDrefExplicitLod = 90U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpImageSampleProjImplicitLod = 91U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpImageSampleProjExplicitLod = 92U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpImageSampleProjDrefImplicitLod = 93U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpImageSampleProjDrefExplicitLod = 94U,
    OpImageFetch = 95U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpImageGather = 96U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpImageDrefGather = 97U,
    OpImageRead = 98U,
    OpImageWrite = 99U,
    OpImage = 100U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpImageQueryFormat = 101U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpImageQueryOrder = 102U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.ImageQuery })]
    OpImageQuerySizeLod = 103U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.ImageQuery })]
    OpImageQuerySize = 104U,
    [DependsOn(Capabilities = new[] { Capability.ImageQuery })]
    OpImageQueryLod = 105U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.ImageQuery })]
    OpImageQueryLevels = 106U,
    [DependsOn(Capabilities = new[] { Capability.Kernel, Capability.ImageQuery })]
    OpImageQuerySamples = 107U,
    OpConvertFToU = 109U,
    OpConvertFToS = 110U,
    OpConvertSToF = 111U,
    OpConvertUToF = 112U,
    OpUConvert = 113U,
    OpSConvert = 114U,
    OpFConvert = 115U,
    OpQuantizeToF16 = 116U,
    [DependsOn(Capabilities = new[] { Capability.Addresses, Capability.PhysicalStorageBufferAddresses })]
    OpConvertPtrToU = 117U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpSatConvertSToU = 118U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpSatConvertUToS = 119U,
    [DependsOn(Capabilities = new[] { Capability.Addresses, Capability.PhysicalStorageBufferAddresses })]
    OpConvertUToPtr = 120U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpPtrCastToGeneric = 121U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpGenericCastToPtr = 122U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpGenericCastToPtrExplicit = 123U,
    OpBitcast = 124U,
    OpSNegate = 126U,
    OpFNegate = 127U,
    OpIAdd = 128U,
    OpFAdd = 129U,
    OpISub = 130U,
    OpFSub = 131U,
    OpIMul = 132U,
    OpFMul = 133U,
    OpUDiv = 134U,
    OpSDiv = 135U,
    OpFDiv = 136U,
    OpUMod = 137U,
    OpSRem = 138U,
    OpSMod = 139U,
    OpFRem = 140U,
    OpFMod = 141U,
    OpVectorTimesScalar = 142U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    OpMatrixTimesScalar = 143U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    OpVectorTimesMatrix = 144U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    OpMatrixTimesVector = 145U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    OpMatrixTimesMatrix = 146U,
    [DependsOn(Capabilities = new[] { Capability.Matrix })]
    OpOuterProduct = 147U,
    OpDot = 148U,
    OpIAddCarry = 149U,
    OpISubBorrow = 150U,
    OpUMulExtended = 151U,
    OpSMulExtended = 152U,
    OpAny = 154U,
    OpAll = 155U,
    OpIsNan = 156U,
    OpIsInf = 157U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpIsFinite = 158U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpIsNormal = 159U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpSignBitSet = 160U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpLessOrGreater = 161U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpOrdered = 162U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpUnordered = 163U,
    OpLogicalEqual = 164U,
    OpLogicalNotEqual = 165U,
    OpLogicalOr = 166U,
    OpLogicalAnd = 167U,
    OpLogicalNot = 168U,
    OpSelect = 169U,
    OpIEqual = 170U,
    OpINotEqual = 171U,
    OpUGreaterThan = 172U,
    OpSGreaterThan = 173U,
    OpUGreaterThanEqual = 174U,
    OpSGreaterThanEqual = 175U,
    OpULessThan = 176U,
    OpSLessThan = 177U,
    OpULessThanEqual = 178U,
    OpSLessThanEqual = 179U,
    OpFOrdEqual = 180U,
    OpFUnordEqual = 181U,
    OpFOrdNotEqual = 182U,
    OpFUnordNotEqual = 183U,
    OpFOrdLessThan = 184U,
    OpFUnordLessThan = 185U,
    OpFOrdGreaterThan = 186U,
    OpFUnordGreaterThan = 187U,
    OpFOrdLessThanEqual = 188U,
    OpFUnordLessThanEqual = 189U,
    OpFOrdGreaterThanEqual = 190U,
    OpFUnordGreaterThanEqual = 191U,
    OpShiftRightLogical = 194U,
    OpShiftRightArithmetic = 195U,
    OpShiftLeftLogical = 196U,
    OpBitwiseOr = 197U,
    OpBitwiseXor = 198U,
    OpBitwiseAnd = 199U,
    OpNot = 200U,
    [DependsOn(Capabilities = new[] { Capability.Shader, Capability.BitInstructions })]
    OpBitFieldInsert = 201U,
    [DependsOn(Capabilities = new[] { Capability.Shader, Capability.BitInstructions })]
    OpBitFieldSExtract = 202U,
    [DependsOn(Capabilities = new[] { Capability.Shader, Capability.BitInstructions })]
    OpBitFieldUExtract = 203U,
    [DependsOn(Capabilities = new[] { Capability.Shader, Capability.BitInstructions })]
    OpBitReverse = 204U,
    OpBitCount = 205U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpDPdx = 207U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpDPdy = 208U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpFwidth = 209U,
    [DependsOn(Capabilities = new[] { Capability.DerivativeControl })]
    OpDPdxFine = 210U,
    [DependsOn(Capabilities = new[] { Capability.DerivativeControl })]
    OpDPdyFine = 211U,
    [DependsOn(Capabilities = new[] { Capability.DerivativeControl })]
    OpFwidthFine = 212U,
    [DependsOn(Capabilities = new[] { Capability.DerivativeControl })]
    OpDPdxCoarse = 213U,
    [DependsOn(Capabilities = new[] { Capability.DerivativeControl })]
    OpDPdyCoarse = 214U,
    [DependsOn(Capabilities = new[] { Capability.DerivativeControl })]
    OpFwidthCoarse = 215U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    OpEmitVertex = 218U,
    [DependsOn(Capabilities = new[] { Capability.Geometry })]
    OpEndPrimitive = 219U,
    [DependsOn(Capabilities = new[] { Capability.GeometryStreams })]
    OpEmitStreamVertex = 220U,
    [DependsOn(Capabilities = new[] { Capability.GeometryStreams })]
    OpEndStreamPrimitive = 221U,
    OpControlBarrier = 224U,
    OpMemoryBarrier = 225U,
    OpAtomicLoad = 227U,
    OpAtomicStore = 228U,
    OpAtomicExchange = 229U,
    OpAtomicCompareExchange = 230U,
    [Obsolete("Last version for this enumerant was 1.3")]
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpAtomicCompareExchangeWeak = 231U,
    OpAtomicIIncrement = 232U,
    OpAtomicIDecrement = 233U,
    OpAtomicIAdd = 234U,
    OpAtomicISub = 235U,
    OpAtomicSMin = 236U,
    OpAtomicUMin = 237U,
    OpAtomicSMax = 238U,
    OpAtomicUMax = 239U,
    OpAtomicAnd = 240U,
    OpAtomicOr = 241U,
    OpAtomicXor = 242U,
    OpPhi = 245U,
    OpLoopMerge = 246U,
    OpSelectionMerge = 247U,
    OpLabel = 248U,
    OpBranch = 249U,
    OpBranchConditional = 250U,
    OpSwitch = 251U,
    [DependsOn(Capabilities = new[] { Capability.Shader })]
    OpKill = 252U,
    OpReturn = 253U,
    OpReturnValue = 254U,
    OpUnreachable = 255U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpLifetimeStart = 256U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpLifetimeStop = 257U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpGroupAsyncCopy = 259U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpGroupWaitEvents = 260U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupAll = 261U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupAny = 262U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupBroadcast = 263U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupIAdd = 264U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupFAdd = 265U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupFMin = 266U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupUMin = 267U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupSMin = 268U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupFMax = 269U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupUMax = 270U,
    [DependsOn(Capabilities = new[] { Capability.Groups })]
    OpGroupSMax = 271U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpReadPipe = 274U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpWritePipe = 275U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpReservedReadPipe = 276U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpReservedWritePipe = 277U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpReserveReadPipePackets = 278U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpReserveWritePipePackets = 279U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpCommitReadPipe = 280U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpCommitWritePipe = 281U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpIsValidReserveId = 282U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpGetNumPipePackets = 283U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpGetMaxPipePackets = 284U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpGroupReserveReadPipePackets = 285U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpGroupReserveWritePipePackets = 286U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpGroupCommitReadPipe = 287U,
    [DependsOn(Capabilities = new[] { Capability.Pipes })]
    OpGroupCommitWritePipe = 288U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpEnqueueMarker = 291U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpEnqueueKernel = 292U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpGetKernelNDrangeSubGroupCount = 293U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpGetKernelNDrangeMaxSubGroupSize = 294U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpGetKernelWorkGroupSize = 295U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpGetKernelPreferredWorkGroupSizeMultiple = 296U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpRetainEvent = 297U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpReleaseEvent = 298U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpCreateUserEvent = 299U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpIsValidEvent = 300U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpSetUserEventStatus = 301U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpCaptureEventProfilingInfo = 302U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpGetDefaultQueue = 303U,
    [DependsOn(Capabilities = new[] { Capability.DeviceEnqueue })]
    OpBuildNDRange = 304U,
    [DependsOn(Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseSampleImplicitLod = 305U,
    [DependsOn(Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseSampleExplicitLod = 306U,
    [DependsOn(Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseSampleDrefImplicitLod = 307U,
    [DependsOn(Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseSampleDrefExplicitLod = 308U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseSampleProjImplicitLod = 309U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseSampleProjExplicitLod = 310U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseSampleProjDrefImplicitLod = 311U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseSampleProjDrefExplicitLod = 312U,
    [DependsOn(Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseFetch = 313U,
    [DependsOn(Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseGather = 314U,
    [DependsOn(Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseDrefGather = 315U,
    [DependsOn(Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseTexelsResident = 316U,
    OpNoLine = 317U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpAtomicFlagTestAndSet = 318U,
    [DependsOn(Capabilities = new[] { Capability.Kernel })]
    OpAtomicFlagClear = 319U,
    [DependsOn(Capabilities = new[] { Capability.SparseResidency })]
    OpImageSparseRead = 320U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.Addresses })]
    OpSizeOf = 321U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.PipeStorage })]
    OpTypePipeStorage = 322U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.PipeStorage })]
    OpConstantPipeStorage = 323U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.PipeStorage })]
    OpCreatePipeFromPipeStorage = 324U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.SubgroupDispatch })]
    OpGetKernelLocalSizeForSubgroupCount = 325U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.SubgroupDispatch })]
    OpGetKernelMaxNumSubgroups = 326U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.NamedBarrier })]
    OpTypeNamedBarrier = 327U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.NamedBarrier })]
    OpNamedBarrierInitialize = 328U,
    [DependsOn(Version = "1.1", Capabilities = new[] { Capability.NamedBarrier })]
    OpMemoryNamedBarrier = 329U,
    [DependsOn(Version = "1.1")]
    OpModuleProcessed = 330U,
    [DependsOn(Version = "1.2")]
    OpExecutionModeId = 331U,
    [DependsOn(Version = "1.2", Extensions = new[] { "SPV_GOOGLE_hlsl_functionality1" })]
    OpDecorateId = 332U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniform })]
    OpGroupNonUniformElect = 333U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformVote })]
    OpGroupNonUniformAll = 334U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformVote })]
    OpGroupNonUniformAny = 335U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformVote })]
    OpGroupNonUniformAllEqual = 336U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformBallot })]
    OpGroupNonUniformBroadcast = 337U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformBallot })]
    OpGroupNonUniformBroadcastFirst = 338U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformBallot })]
    OpGroupNonUniformBallot = 339U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformBallot })]
    OpGroupNonUniformInverseBallot = 340U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformBallot })]
    OpGroupNonUniformBallotBitExtract = 341U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformBallot })]
    OpGroupNonUniformBallotBitCount = 342U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformBallot })]
    OpGroupNonUniformBallotFindLSB = 343U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformBallot })]
    OpGroupNonUniformBallotFindMSB = 344U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformShuffle })]
    OpGroupNonUniformShuffle = 345U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformShuffle })]
    OpGroupNonUniformShuffleXor = 346U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformShuffleRelative })]
    OpGroupNonUniformShuffleUp = 347U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformShuffleRelative })]
    OpGroupNonUniformShuffleDown = 348U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformIAdd = 349U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformFAdd = 350U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformIMul = 351U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformFMul = 352U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformSMin = 353U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformUMin = 354U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformFMin = 355U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformSMax = 356U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformUMax = 357U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformFMax = 358U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformBitwiseAnd = 359U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformBitwiseOr = 360U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformBitwiseXor = 361U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformLogicalAnd = 362U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformLogicalOr = 363U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformArithmetic, Capability.GroupNonUniformClustered, Capability.GroupNonUniformPartitionedNV })]
    OpGroupNonUniformLogicalXor = 364U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformQuad })]
    OpGroupNonUniformQuadBroadcast = 365U,
    [DependsOn(Version = "1.3", Capabilities = new[] { Capability.GroupNonUniformQuad })]
    OpGroupNonUniformQuadSwap = 366U,
    [DependsOn(Version = "1.4")]
    OpCopyLogical = 400U,
    [DependsOn(Version = "1.4")]
    OpPtrEqual = 401U,
    [DependsOn(Version = "1.4")]
    OpPtrNotEqual = 402U,
    [DependsOn(Version = "1.4", Capabilities = new[] { Capability.Addresses, Capability.VariablePointers, Capability.VariablePointersStorageBuffer })]
    OpPtrDiff = 403U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Shader }, Extensions = new[] { "SPV_KHR_terminate_invocation" })]
    OpTerminateInvocation = 4416U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupBallotKHR }, Extensions = new[] { "SPV_KHR_shader_ballot" })]
    OpSubgroupBallotKHR = 4421U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupBallotKHR }, Extensions = new[] { "SPV_KHR_shader_ballot" })]
    OpSubgroupFirstInvocationKHR = 4422U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupVoteKHR }, Extensions = new[] { "SPV_KHR_subgroup_vote" })]
    OpSubgroupAllKHR = 4428U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupVoteKHR }, Extensions = new[] { "SPV_KHR_subgroup_vote" })]
    OpSubgroupAnyKHR = 4429U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupVoteKHR }, Extensions = new[] { "SPV_KHR_subgroup_vote" })]
    OpSubgroupAllEqualKHR = 4430U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupBallotKHR }, Extensions = new[] { "SPV_KHR_shader_ballot" })]
    OpSubgroupReadInvocationKHR = 4432U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingKHR }, Extensions = new[] { "SPV_KHR_ray_tracing" })]
    OpTraceRayKHR = 4445U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingKHR }, Extensions = new[] { "SPV_KHR_ray_tracing" })]
    OpExecuteCallableKHR = 4446U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingKHR, Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_tracing", "SPV_KHR_ray_query" })]
    OpConvertUToAccelerationStructureKHR = 4447U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingKHR }, Extensions = new[] { "SPV_KHR_ray_tracing" })]
    OpIgnoreIntersectionKHR = 4448U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingKHR }, Extensions = new[] { "SPV_KHR_ray_tracing" })]
    OpTerminateRayKHR = 4449U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.DotProductKHR })]
    OpSDotKHR = 4450U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.DotProductKHR })]
    OpUDotKHR = 4451U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.DotProductKHR })]
    OpSUDotKHR = 4452U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.DotProductKHR })]
    OpSDotAccSatKHR = 4453U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.DotProductKHR })]
    OpUDotAccSatKHR = 4454U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.DotProductKHR })]
    OpSUDotAccSatKHR = 4455U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpTypeRayQueryKHR = 4472U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryInitializeKHR = 4473U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryTerminateKHR = 4474U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGenerateIntersectionKHR = 4475U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryConfirmIntersectionKHR = 4476U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryProceedKHR = 4477U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionTypeKHR = 4479U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Groups }, Extensions = new[] { "SPV_AMD_shader_ballot" })]
    OpGroupIAddNonUniformAMD = 5000U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Groups }, Extensions = new[] { "SPV_AMD_shader_ballot" })]
    OpGroupFAddNonUniformAMD = 5001U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Groups }, Extensions = new[] { "SPV_AMD_shader_ballot" })]
    OpGroupFMinNonUniformAMD = 5002U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Groups }, Extensions = new[] { "SPV_AMD_shader_ballot" })]
    OpGroupUMinNonUniformAMD = 5003U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Groups }, Extensions = new[] { "SPV_AMD_shader_ballot" })]
    OpGroupSMinNonUniformAMD = 5004U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Groups }, Extensions = new[] { "SPV_AMD_shader_ballot" })]
    OpGroupFMaxNonUniformAMD = 5005U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Groups }, Extensions = new[] { "SPV_AMD_shader_ballot" })]
    OpGroupUMaxNonUniformAMD = 5006U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.Groups }, Extensions = new[] { "SPV_AMD_shader_ballot" })]
    OpGroupSMaxNonUniformAMD = 5007U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentMaskAMD }, Extensions = new[] { "SPV_AMD_shader_fragment_mask" })]
    OpFragmentMaskFetchAMD = 5011U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentMaskAMD }, Extensions = new[] { "SPV_AMD_shader_fragment_mask" })]
    OpFragmentFetchAMD = 5012U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ShaderClockKHR }, Extensions = new[] { "SPV_KHR_shader_clock" })]
    OpReadClockKHR = 5056U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ImageFootprintNV }, Extensions = new[] { "SPV_NV_shader_image_footprint" })]
    OpImageSampleFootprintNV = 5283U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.GroupNonUniformPartitionedNV }, Extensions = new[] { "SPV_NV_shader_subgroup_partitioned" })]
    OpGroupNonUniformPartitionNV = 5296U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.MeshShadingNV }, Extensions = new[] { "SPV_NV_mesh_shader" })]
    OpWritePackedPrimitiveIndices4x8NV = 5299U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    OpReportIntersectionNV = 5334U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing" })]
    OpReportIntersectionKHR = 5334U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV }, Extensions = new[] { "SPV_NV_ray_tracing" })]
    OpIgnoreIntersectionNV = 5335U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV }, Extensions = new[] { "SPV_NV_ray_tracing" })]
    OpTerminateRayNV = 5336U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV }, Extensions = new[] { "SPV_NV_ray_tracing" })]
    OpTraceNV = 5337U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingMotionBlurNV }, Extensions = new[] { "SPV_NV_ray_tracing_motion_blur" })]
    OpTraceMotionNV = 5338U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingMotionBlurNV }, Extensions = new[] { "SPV_NV_ray_tracing_motion_blur" })]
    OpTraceRayMotionNV = 5339U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR, Capability.RayQueryKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing", "SPV_KHR_ray_query" })]
    OpTypeAccelerationStructureNV = 5341U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV, Capability.RayTracingKHR, Capability.RayQueryKHR }, Extensions = new[] { "SPV_NV_ray_tracing", "SPV_KHR_ray_tracing", "SPV_KHR_ray_query" })]
    OpTypeAccelerationStructureKHR = 5341U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayTracingNV }, Extensions = new[] { "SPV_NV_ray_tracing" })]
    OpExecuteCallableNV = 5344U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.CooperativeMatrixNV }, Extensions = new[] { "SPV_NV_cooperative_matrix" })]
    OpTypeCooperativeMatrixNV = 5358U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.CooperativeMatrixNV }, Extensions = new[] { "SPV_NV_cooperative_matrix" })]
    OpCooperativeMatrixLoadNV = 5359U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.CooperativeMatrixNV }, Extensions = new[] { "SPV_NV_cooperative_matrix" })]
    OpCooperativeMatrixStoreNV = 5360U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.CooperativeMatrixNV }, Extensions = new[] { "SPV_NV_cooperative_matrix" })]
    OpCooperativeMatrixMulAddNV = 5361U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.CooperativeMatrixNV }, Extensions = new[] { "SPV_NV_cooperative_matrix" })]
    OpCooperativeMatrixLengthNV = 5362U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShaderSampleInterlockEXT, Capability.FragmentShaderPixelInterlockEXT, Capability.FragmentShaderShadingRateInterlockEXT }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    OpBeginInvocationInterlockEXT = 5364U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FragmentShaderSampleInterlockEXT, Capability.FragmentShaderPixelInterlockEXT, Capability.FragmentShaderShadingRateInterlockEXT }, Extensions = new[] { "SPV_EXT_fragment_shader_interlock" })]
    OpEndInvocationInterlockEXT = 5365U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.DemoteToHelperInvocationEXT }, Extensions = new[] { "SPV_EXT_demote_to_helper_invocation" })]
    OpDemoteToHelperInvocationEXT = 5380U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.DemoteToHelperInvocationEXT }, Extensions = new[] { "SPV_EXT_demote_to_helper_invocation" })]
    OpIsHelperInvocationEXT = 5381U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupShuffleINTEL })]
    OpSubgroupShuffleINTEL = 5571U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupShuffleINTEL })]
    OpSubgroupShuffleDownINTEL = 5572U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupShuffleINTEL })]
    OpSubgroupShuffleUpINTEL = 5573U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupShuffleINTEL })]
    OpSubgroupShuffleXorINTEL = 5574U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupBufferBlockIOINTEL })]
    OpSubgroupBlockReadINTEL = 5575U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupBufferBlockIOINTEL })]
    OpSubgroupBlockWriteINTEL = 5576U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupImageBlockIOINTEL })]
    OpSubgroupImageBlockReadINTEL = 5577U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupImageBlockIOINTEL })]
    OpSubgroupImageBlockWriteINTEL = 5578U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupImageMediaBlockIOINTEL })]
    OpSubgroupImageMediaBlockReadINTEL = 5580U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.SubgroupImageMediaBlockIOINTEL })]
    OpSubgroupImageMediaBlockWriteINTEL = 5581U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpUCountLeadingZerosINTEL = 5585U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpUCountTrailingZerosINTEL = 5586U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpAbsISubINTEL = 5587U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpAbsUSubINTEL = 5588U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpIAddSatINTEL = 5589U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpUAddSatINTEL = 5590U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpIAverageINTEL = 5591U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpUAverageINTEL = 5592U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpIAverageRoundedINTEL = 5593U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpUAverageRoundedINTEL = 5594U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpISubSatINTEL = 5595U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpUSubSatINTEL = 5596U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpIMul32x16INTEL = 5597U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.IntegerFunctions2INTEL })]
    OpUMul32x16INTEL = 5598U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.AtomicFloat16MinMaxEXT, Capability.AtomicFloat32MinMaxEXT, Capability.AtomicFloat64MinMaxEXT })]
    OpAtomicFMinEXT = 5614U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.AtomicFloat16MinMaxEXT, Capability.AtomicFloat32MinMaxEXT, Capability.AtomicFloat64MinMaxEXT })]
    OpAtomicFMaxEXT = 5615U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ExpectAssumeKHR }, Extensions = new[] { "SPV_KHR_expect_assume" })]
    OpAssumeTrueKHR = 5630U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.ExpectAssumeKHR }, Extensions = new[] { "SPV_KHR_expect_assume" })]
    OpExpectKHR = 5631U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_GOOGLE_decorate_string", "SPV_GOOGLE_hlsl_functionality1" })]
    OpDecorateString = 5632U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_GOOGLE_decorate_string", "SPV_GOOGLE_hlsl_functionality1" })]
    OpDecorateStringGOOGLE = 5632U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_GOOGLE_decorate_string", "SPV_GOOGLE_hlsl_functionality1" })]
    OpMemberDecorateString = 5633U,
    [DependsOn(Version = "1.4", Extensions = new[] { "SPV_GOOGLE_decorate_string", "SPV_GOOGLE_hlsl_functionality1" })]
    OpMemberDecorateStringGOOGLE = 5633U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.UnstructuredLoopControlsINTEL }, Extensions = new[] { "SPV_INTEL_unstructured_loop_controls" })]
    OpLoopControlINTEL = 5887U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.BlockingPipesINTEL }, Extensions = new[] { "SPV_INTEL_blocking_pipes" })]
    OpReadPipeBlockingINTEL = 5946U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.BlockingPipesINTEL }, Extensions = new[] { "SPV_INTEL_blocking_pipes" })]
    OpWritePipeBlockingINTEL = 5947U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.FPGARegINTEL }, Extensions = new[] { "SPV_INTEL_fpga_reg" })]
    OpFPGARegINTEL = 5949U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetRayTMinKHR = 6016U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetRayFlagsKHR = 6017U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionTKHR = 6018U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionInstanceCustomIndexKHR = 6019U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionInstanceIdKHR = 6020U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionInstanceShaderBindingTableRecordOffsetKHR = 6021U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionGeometryIndexKHR = 6022U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionPrimitiveIndexKHR = 6023U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionBarycentricsKHR = 6024U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionFrontFaceKHR = 6025U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionCandidateAABBOpaqueKHR = 6026U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionObjectRayDirectionKHR = 6027U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionObjectRayOriginKHR = 6028U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetWorldRayDirectionKHR = 6029U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetWorldRayOriginKHR = 6030U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionObjectToWorldKHR = 6031U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.RayQueryKHR }, Extensions = new[] { "SPV_KHR_ray_query" })]
    OpRayQueryGetIntersectionWorldToObjectKHR = 6032U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.AtomicFloat16AddEXT, Capability.AtomicFloat32AddEXT, Capability.AtomicFloat64AddEXT }, Extensions = new[] { "SPV_EXT_shader_atomic_float_add" })]
    OpAtomicFAddEXT = 6035U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.VectorComputeINTEL })]
    OpTypeBufferSurfaceINTEL = 6086U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.LongConstantCompositeINTEL })]
    OpTypeStructContinuedINTEL = 6090U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.LongConstantCompositeINTEL })]
    OpConstantCompositeContinuedINTEL = 6091U,
    [DependsOn(Version = "None", Capabilities = new[] { Capability.LongConstantCompositeINTEL })]
    OpSpecConstantCompositeContinuedINTEL = 6092U,
}

