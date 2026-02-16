namespace UnityEngine.XR.OpenXR.Features.Android
{
    /// <summary>
    /// Must match AXR native type, XrFaceParameterIndicesANDROID
    ///
    /// Enum values that identify the face action units affecting the expression on the face.
    /// </summary>
    /// <remarks>Each action unit corresponds to a facial feature that can move. A coefficient of zero for the
    /// feature represents the neutral position, while a coefficient of one represents the fully articulated
    /// position.
    ///
    /// Call <see cref="AndroidOpenXRFaceSubsystem.GetChanges*"/> to get the coefficients change for a given face
    /// </remarks>
    public enum AndroidXRBlendShapeLocation
    {
        /// <summary>
        /// The left brow lowerer blendshape parameter.
        /// </summary>
        BROW_LOWERER_L = 0,
        /// <summary>
        /// The right brow lowerer blendshape parameter.
        /// </summary>
        BROW_LOWERER_R = 1,
        /// <summary>
        /// The left cheek puff blendshape parameter.
        /// </summary>
        CHEEK_PUFF_L = 2,
        /// <summary>
        /// The right cheek puff blendshape parameter.
        /// </summary>
        CHEEK_PUFF_R = 3,
        /// <summary>
        /// The left cheek raiser blendshape parameter.
        /// </summary>
        CHEEK_RAISER_L = 4,
        /// <summary>
        /// The right cheek raiser blendshape parameter.
        /// </summary>
        CHEEK_RAISER_R = 5,
        /// <summary>
        /// The left cheek suck blendshape parameter.
        /// </summary>
        CHEEK_SUCK_L = 6,
        /// <summary>
        /// The right cheek suck blendshape parameter.
        /// </summary>
        CHEEK_SUCK_R = 7,
        /// <summary>
        /// The bottom chin raiser blendshape parameter.
        /// </summary>
        CHIN_RAISER_B = 8,
        /// <summary>
        /// The top chin raiser blendshape parameter.
        /// </summary>
        CHIN_RAISER_T = 9,
        /// <summary>
        /// The left dimpler blendshape parameter.
        /// </summary>
        DIMPLER_L = 10,
        /// <summary>
        /// The right dimpler lowerer blendshape parameter.
        /// </summary>
        DIMPLER_R = 11,
        /// <summary>
        /// The left eyes closed blendshape parameter.
        /// </summary>
        EYES_CLOSED_L = 12,
        /// <summary>
        /// The right eyes closed blendshape parameter.
        /// </summary>
        EYES_CLOSED_R = 13,
        /// <summary>
        /// The left eyes look down blendshape parameter.
        /// </summary>
        EYES_LOOK_DOWN_L = 14,
        /// <summary>
        /// The right eyes look down blendshape parameter.
        /// </summary>
        EYES_LOOK_DOWN_R = 15,
        /// <summary>
        /// The left look left blendshape parameter.
        /// </summary>
        EYES_LOOK_LEFT_L = 16,
        /// <summary>
        /// The left look right blendshape parameter.
        /// </summary>
        EYES_LOOK_LEFT_R = 17,
        /// <summary>
        /// The right look left blendshape parameter.
        /// </summary>
        EYES_LOOK_RIGHT_L = 18,
        /// <summary>
        /// The right look right blendshape parameter.
        /// </summary>
        EYES_LOOK_RIGHT_R = 19,
        /// <summary>
        /// The left eyes look up blendshape parameter.
        /// </summary>
        EYES_LOOK_UP_L = 20,
        /// <summary>
        /// The right eyes look up blendshape parameter.
        /// </summary>
        EYES_LOOK_UP_R = 21,
        /// <summary>
        /// The left inner brow raiser blendshape parameter.
        /// </summary>
        INNER_BROW_RAISER_L = 22,
        /// <summary>
        /// The right inner brow raiser blendshape parameter.
        /// </summary>
        INNER_BROW_RAISER_R = 23,
        /// <summary>
        /// The jaw drop blendshape parameter.
        /// </summary>
        JAW_DROP = 24,
        /// <summary>
        /// The left jaw sideways blendshape parameter.
        /// </summary>
        JAW_SIDEWAYS_LEFT = 25,
        /// <summary>
        /// The right jaw sideways blendshape parameter.
        /// </summary>
        JAW_SIDEWAYS_RIGHT = 26,
        /// <summary>
        /// The jaw thrust blendshape parameter.
        /// </summary>
        JAW_THRUST = 27,
        /// <summary>
        /// The left lid tightener blendshape parameter.
        /// </summary>
        LID_TIGHTENER_L = 28,
        /// <summary>
        /// The right lid tightener blendshape parameter.
        /// </summary>
        LID_TIGHTENER_R = 29,
        /// <summary>
        /// The left corner lip depressor blendshape parameter.
        /// </summary>
        LIP_CORNER_DEPRESSOR_L = 30,
        /// <summary>
        /// The right corner lip depressor blendshape parameter.
        /// </summary>
        LIP_CORNER_DEPRESSOR_R = 31,
        /// <summary>
        /// The left corner lip puller blendshape parameter.
        /// </summary>
        LIP_CORNER_PULLER_L = 32,
        /// <summary>
        /// The right corner lip puller blendshape parameter.
        /// </summary>
        LIP_CORNER_PULLER_R = 33,
        /// <summary>
        /// The left bottom lip funneler blendshape parameter.
        /// </summary>
        LIP_FUNNELER_LB = 34,
        /// <summary>
        /// The left top lip funneler blendshape parameter.
        /// </summary>
        LIP_FUNNELER_LT = 35,
        /// <summary>
        /// The right bottom lip funneler blendshape parameter.
        /// </summary>
        LIP_FUNNELER_RB = 36,
        /// <summary>
        /// The right top lip funneler blendshape parameter.
        /// </summary>
        LIP_FUNNELER_RT = 37,
        /// <summary>
        /// The left lip pressor blendshape parameter.
        /// </summary>
        LIP_PRESSOR_L = 38,
        /// <summary>
        /// The right lip pressor blendshape parameter.
        /// </summary>
        LIP_PRESSOR_R = 39,
        /// <summary>
        /// The left lip pucker blendshape parameter.
        /// </summary>
        LIP_PUCKER_L = 40,
        /// <summary>
        /// The right lip pucker blendshape parameter.
        /// </summary>
        LIP_PUCKER_R = 41,
        /// <summary>
        /// The left lip stretcher blendshape parameter.
        /// </summary>
        LIP_STRETCHER_L = 42,
        /// <summary>
        /// The right lip stretcher blendshape parameter.
        /// </summary>
        LIP_STRETCHER_R = 43,
        /// <summary>
        /// The left bottom lip suck blendshape parameter.
        /// </summary>
        LIP_SUCK_LB = 44,
        /// <summary>
        /// The left top lip suck blendshape parameter.
        /// </summary>
        LIP_SUCK_LT = 45,
        /// <summary>
        /// The right bottom lip suck blendshape parameter.
        /// </summary>
        LIP_SUCK_RB = 46,
        /// <summary>
        /// The right top lip suck blendshape parameter.
        /// </summary>
        LIP_SUCK_RT = 47,
        /// <summary>
        /// The left lip tightener blendshape parameter.
        /// </summary>
        LIP_TIGHTENER_L = 48,
        /// <summary>
        /// The right lip tightener blendshape parameter.
        /// </summary>
        LIP_TIGHTENER_R = 49,
        /// <summary>
        /// The lips toward blendshape parameter.
        /// </summary>
        LIPS_TOWARD = 50,
        /// <summary>
        /// The left lower lip depressor blendshape parameter.
        /// </summary>
        LOWER_LIP_DEPRESSOR_L = 51,
        /// <summary>
        /// The right lower lip depressor blendshape parameter.
        /// </summary>
        LOWER_LIP_DEPRESSOR_R = 52,
        /// <summary>
        /// The mouth move left blendshape parameter.
        /// </summary>
        MOUTH_LEFT = 53,
        /// <summary>
        /// The mouth move right blendshape parameter.
        /// </summary>
        MOUTH_RIGHT = 54,
        /// <summary>
        /// The left nose wrinkler blendshape parameter.
        /// </summary>
        NOSE_WRINKLER_L = 55,
        /// <summary>
        /// The right nose wrinkler blendshape parameter.
        /// </summary>
        NOSE_WRINKLER_R = 56,
        /// <summary>
        /// The left outer brow raiser blendshape parameter.
        /// </summary>
        OUTER_BROW_RAISER_L = 57,
        /// <summary>
        /// The right outer brow raiser blendshape parameter.
        /// </summary>
        OUTER_BROW_RAISER_R = 58,
        /// <summary>
        /// The left lid raiser blendshape parameter.
        /// </summary>
        UPPER_LID_RAISER_L = 59,
        /// <summary>
        /// The right lid raiser blendshape parameter.
        /// </summary>
        UPPER_LID_RAISER_R = 60,
        /// <summary>
        /// The left lip raiser blendshape parameter.
        /// </summary>
        UPPER_LIP_RAISER_L = 61,
        /// <summary>
        /// The right lip raiser blendshape parameter.
        /// </summary>
        UPPER_LIP_RAISER_R = 62,
        /// <summary>
        /// The tongue out blendshape parameter.
        /// </summary>
        TONGUE_OUT = 63,
        /// <summary>
        /// The tongue left puller blendshape parameter.
        /// </summary>
        TONGUE_LEFT = 64,
        /// <summary>
        /// The right right puller blendshape parameter.
        /// </summary>
        TONGUE_RIGHT = 65,
        /// <summary>
        /// The right up puller blendshape parameter.
        /// </summary>
        TONGUE_UP = 66,
        /// <summary>
        /// The right down puller blendshape parameter.
        /// </summary>
        TONGUE_DOWN = 67
    }
}
