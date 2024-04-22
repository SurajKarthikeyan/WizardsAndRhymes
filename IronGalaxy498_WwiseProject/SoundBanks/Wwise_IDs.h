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
        static const AkUniqueID BLASTATTACK = 3442485957U;
        static const AkUniqueID BUTTONHOVER = 3035572085U;
        static const AkUniqueID BUTTONPRESS = 317641954U;
        static const AkUniqueID CROWDSURF = 1739654744U;
        static const AkUniqueID CRYSTAL = 3444057113U;
        static const AkUniqueID DASHREADY = 1022374206U;
        static const AkUniqueID FART = 2948602592U;
        static const AkUniqueID GATELIFT = 1960186623U;
        static const AkUniqueID HAUNTEDSTINGER = 1950078070U;
        static const AkUniqueID ICEBLOCK = 418751485U;
        static const AkUniqueID IFORGOTWHATAPRESSUREPLATEWASCALLED = 2494836859U;
        static const AkUniqueID LEVER = 2782712987U;
        static const AkUniqueID MELEEFIRE = 2248677623U;
        static const AkUniqueID MELEELIGHTNING = 1583612915U;
        static const AkUniqueID METALBLOCKPUSH = 3570861977U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID PLAYBUTTON = 1843531235U;
        static const AkUniqueID PLAYERDASH = 2525052962U;
        static const AkUniqueID PLAYERHURT = 3537581393U;
        static const AkUniqueID PLAYERICESLIDE = 816354274U;
        static const AkUniqueID RANGEDFIRE = 1744863420U;
        static const AkUniqueID RANGEDLIGHTNING = 1899629126U;
        static const AkUniqueID SKELETONHIT = 2460311919U;
        static const AkUniqueID TESLACOILOFF = 2261469182U;
        static const AkUniqueID WORDPICKUP = 3290622745U;
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

        namespace METALBLOCKPUSH
        {
            static const AkUniqueID GROUP = 3570861977U;

            namespace STATE
            {
                static const AkUniqueID ISPUSHING = 2597476059U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID NOTPUSHING = 91517132U;
            } // namespace STATE
        } // namespace METALBLOCKPUSH

        namespace MUSICSTATE
        {
            static const AkUniqueID GROUP = 1021618141U;

            namespace STATE
            {
                static const AkUniqueID ICECOMBAT = 733150682U;
                static const AkUniqueID ICEOUTOFCOMBAT = 3099519893U;
                static const AkUniqueID ICEOUTOFPUZZLE = 4102602869U;
                static const AkUniqueID ICEPUZZLE = 192579722U;
                static const AkUniqueID INTRO = 1125500713U;
                static const AkUniqueID LIGHTNINGCOMBAT = 2990546425U;
                static const AkUniqueID LIGHTNINGOUTOFCOMBAT = 4114645792U;
                static const AkUniqueID LIGHTNINGOUTOFPUZZLE = 2437783708U;
                static const AkUniqueID LIGHTNINGPUZZLE = 3485352473U;
                static const AkUniqueID MAINMENU = 3604647259U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace MUSICSTATE

        namespace PLAYERONICE
        {
            static const AkUniqueID GROUP = 3506000894U;

            namespace STATE
            {
                static const AkUniqueID ISSLIDING = 437705085U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID NOTSLIDING = 3078077546U;
            } // namespace STATE
        } // namespace PLAYERONICE

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID AMP = 1117531621U;
        static const AkUniqueID HIGH = 3550808449U;
        static const AkUniqueID LOW = 545371365U;
        static const AkUniqueID MASTERVOLUME = 2918011349U;
        static const AkUniqueID MID = 1182670505U;
    } // namespace GAME_PARAMETERS

    namespace TRIGGERS
    {
        static const AkUniqueID HAUNTEDDROPSTINGER = 4275975763U;
    } // namespace TRIGGERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID COMBATANDENVIRONMENT = 3361862619U;
        static const AkUniqueID INTERACTIVEMUSICBANK = 1702439946U;
        static const AkUniqueID UIANDFARTSOUNDBANK = 784481870U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID DIALOGUEBUS = 309504875U;
        static const AkUniqueID ENVIRONMENTBUS = 221867874U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSICBUS = 2886307548U;
        static const AkUniqueID SOUNDFXBUS = 3427258114U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
