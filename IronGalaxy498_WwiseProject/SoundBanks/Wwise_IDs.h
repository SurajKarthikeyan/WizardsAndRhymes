/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID BUTTONHOVER = 3035572085U;
        static const AkUniqueID BUTTONPRESS = 317641954U;
        static const AkUniqueID CROWDSURF = 1739654744U;
        static const AkUniqueID CRYSTAL = 3444057113U;
        static const AkUniqueID FREQTEST = 1061604899U;
        static const AkUniqueID ICEBLOCK = 418751485U;
        static const AkUniqueID LEVER = 2782712987U;
        static const AkUniqueID MELEEFIRE = 2248677623U;
        static const AkUniqueID MELEEICE = 1651067412U;
        static const AkUniqueID MELEELIGHTNING = 1583612915U;
        static const AkUniqueID PLAY_TESTDOOROPEN = 3831721246U;
        static const AkUniqueID PLAYBUTTON = 1843531235U;
        static const AkUniqueID PLAYERHURT = 3537581393U;
        static const AkUniqueID PROTOCOMBATLOOP = 3855831843U;
        static const AkUniqueID RANGEDFIRE = 1744863420U;
        static const AkUniqueID RANGEDICE = 2982846157U;
        static const AkUniqueID RANGEDLIGHTNING = 1899629126U;
        static const AkUniqueID ROCKPLATFORMPUZZLE = 1518733879U;
        static const AkUniqueID ROOMMUS = 427386941U;
        static const AkUniqueID SKELETONHIT = 2460311919U;
        static const AkUniqueID TESTEVENT = 1097980931U;
        static const AkUniqueID TESTINTERACTIVEMUSIC = 372047860U;
        static const AkUniqueID TESTLEVEREVENT = 1177918645U;
        static const AkUniqueID TESTMUSIC = 1324413170U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace CROWDSURFING
        {
            static const AkUniqueID GROUP = 2352559104U;

            namespace STATE
            {
                static const AkUniqueID INCROWDSURF = 2260458779U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID OUTOFCROWDSURF = 2475500331U;
            } // namespace STATE
        } // namespace CROWDSURFING

        namespace MUSICSTATE
        {
            static const AkUniqueID GROUP = 1021618141U;

            namespace STATE
            {
                static const AkUniqueID COMBAT = 2764240573U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PUZZLE = 1780448749U;
            } // namespace STATE
        } // namespace MUSICSTATE

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID AMP = 1117531621U;
        static const AkUniqueID HIGH = 3550808449U;
        static const AkUniqueID LOW = 545371365U;
        static const AkUniqueID MASTERVOLUME = 2918011349U;
        static const AkUniqueID MID = 1182670505U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID DIALOGUEBUS = 309504875U;
        static const AkUniqueID ENVIRONMENTBUS = 221867874U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSICBUS = 2886307548U;
        static const AkUniqueID SOUNDFXBUS = 3427258114U;
        static const AkUniqueID TESTBUS = 1966988073U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID AUXHIGH = 221139265U;
        static const AkUniqueID AUXLOW = 411780261U;
        static const AkUniqueID AUXMID = 1049182825U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
