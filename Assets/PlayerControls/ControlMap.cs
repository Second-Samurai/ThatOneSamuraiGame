// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls/ControlMap.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ControlMap : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControlMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControlMap"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""03a7faf7-48b0-4ff4-9a54-30dbdb5a2c3f"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""95c8a337-6c92-4696-8716-0e1f9ceb135f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LockOn"",
                    ""type"": ""Button"",
                    ""id"": ""7d3b2d0c-6ef1-4db4-85ab-f1365c38ebdd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""RotateCamera"",
                    ""type"": ""Value"",
                    ""id"": ""4583ff09-6513-4aca-b152-4d0df10adb13"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StartBlock"",
                    ""type"": ""Button"",
                    ""id"": ""bbaa54ba-ee2d-4bfa-9050-1e80af91e213"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""EndBlock"",
                    ""type"": ""Button"",
                    ""id"": ""155d1532-4f55-42b2-abd4-2a8dceb44249"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""bb4d4c72-aca8-4669-a79e-e0b19c891ba1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""f1e70147-378b-429e-a976-ca9e31227a1e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""InitRewind"",
                    ""type"": ""Button"",
                    ""id"": ""3be9406a-147b-42e2-9464-c8a788c65bb7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ScrubForwardOLD"",
                    ""type"": ""Value"",
                    ""id"": ""fb68f0ce-984f-4167-8e4d-d9c07aee1e24"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""ScrubBackwardOLD"",
                    ""type"": ""Value"",
                    ""id"": ""fd6e816c-1091-4ea7-9a26-79325a93cb37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""ScrubOLD"",
                    ""type"": ""Value"",
                    ""id"": ""a01967d2-ed35-4228-81c7-8f9483263fb6"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ReleaseLockOn"",
                    ""type"": ""Button"",
                    ""id"": ""49a3647a-04d7-4753-9c18-a0dfb073b98b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ToggleLockLeft"",
                    ""type"": ""Button"",
                    ""id"": ""bd0d209c-7483-408c-a191-99172ebbb538"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ToggleLockRight"",
                    ""type"": ""Button"",
                    ""id"": ""5661c98b-a309-46eb-a33a-5d0c715004c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""StartHeavy"",
                    ""type"": ""Button"",
                    ""id"": ""29d73101-4da2-4199-99f3-8f4d7d6cd25e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.3,pressPoint=0.5),Press""
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""58f1a698-757e-425c-aefc-947683b1f9ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwordDraw"",
                    ""type"": ""Button"",
                    ""id"": ""1440196e-ef2a-4f79-abc3-8b6201a2d8d9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""1904602d-c2c1-4685-8892-8a0074e63743"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""MoveVector"",
                    ""id"": ""cd7dad81-1e2b-4794-a970-7aa979b37318"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""df4fb3ce-b87c-4e96-a365-12f641ed7f36"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""72b39b23-f003-4eb9-a16e-c292f2b7f410"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""832455c4-83dc-499d-aee3-b96e49a0d080"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""048c37c2-6500-47ac-a736-ccfa37be4c3e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ae09063b-57f4-4ea6-b1eb-c0aae526c889"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""876fd3b0-82c9-4ccd-b1d5-973bc6827c1e"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f1517d20-5872-4708-91b1-6c8462407f34"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e15cbb6d-472d-4c3e-ac02-8c8d1413ab6c"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5da429b8-1e92-446b-9d1b-f0492745f479"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7872b9b-acf4-4d23-a0f6-36bf4fa2cc74"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""LockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1d97cbf-ba02-4e42-b4e9-d064fb9e725c"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37af880b-3068-4ec7-8897-0736725c59fc"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c13caa9-6cca-4117-a0f2-545ef375dea7"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=50,y=50)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85801459-01e9-41ef-975d-f7fc78753f35"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""StartBlock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fab1d046-c9b5-4ccf-8af7-4a16bd769748"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""StartBlock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34d6019d-9add-40c5-a865-4485e9576556"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""798e952a-2f7d-458e-8499-fa34d44e1b7f"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c6e712e-b27f-449d-9210-c50c19184778"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""EndBlock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e242a578-3f0e-4434-8b9d-9cfad5be29f0"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""EndBlock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4a4f5ef-45b6-4a1d-919c-3fad052b9930"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5aa8508c-ef6c-49c7-8f2c-7b5cd973a490"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad1995c5-6434-433f-8702-59a1d673bad0"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""InitRewind"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90e30185-8f51-4e3a-9e3f-626a194d9159"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""InitRewind"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc5e184d-6722-4d6e-97f0-d75def961161"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ScrubForwardOLD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd0f1acd-6b1b-4150-a5cd-4a454c189c1c"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ScrubForwardOLD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f09dedd-5f9b-46bc-8720-6887997f196b"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ScrubBackwardOLD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f2feca7-2f03-4442-8343-287a3eb418e6"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ScrubBackwardOLD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""66fc671d-2d06-47a2-a4f4-0377b90c409e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ScrubOLD"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""406c72db-3bd6-4996-91fb-f2140230d46e"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ScrubOLD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""cd86892c-a2ac-4645-ae17-859e3587665b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ScrubOLD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5cb0fe6b-e53a-4704-8767-f338b4ed220f"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ReleaseLockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bd73465-bdab-4f3c-b20d-3dec929e7847"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ReleaseLockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ce452a9-9034-40a7-8c4b-138d4c515200"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ToggleLockLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2082f3f7-03f8-42eb-815f-a5d89470f492"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ToggleLockLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21ee589b-97f0-4fe1-a5df-1b5c266a9596"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ToggleLockRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0621afc2-aee6-44b0-8e00-8deaa209351f"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ToggleLockRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17ad6dd6-9a2a-4a9e-9dfa-bf9f27fa785b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""StartHeavy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d0615405-a612-4b1d-96c2-7077a3bc04d2"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""StartHeavy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc95f931-6a77-4210-87eb-ba4358b8a391"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba895857-51d3-41d4-b1e0-0eaf4e766be1"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86337c31-ce3f-48d8-97d2-eba2580d693b"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88748e90-f758-4c42-8382-689e5405c730"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Rewind"",
            ""id"": ""de798162-45d1-45f2-ab63-79fef9aab4a3"",
            ""actions"": [
                {
                    ""name"": ""Scrub"",
                    ""type"": ""Value"",
                    ""id"": ""20d4465d-c314-49ba-bbee-0182099a77d1"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""bb9ae5c8-0b93-44e0-b002-2b0380f4e35a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EndRewind"",
                    ""type"": ""Button"",
                    ""id"": ""79a9d872-5340-4333-8f11-7577c02c03cf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""1f59be54-64fb-480c-b051-21570d1b079c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scrub"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""aaa1bd4e-05e2-49dd-9d34-ed98cca85ba4"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Scrub"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""04c2f116-7a8f-4f84-aaba-0d2aab4a0882"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Scrub"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7ee15c39-35af-42ed-b9b3-7331e19b09fb"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""EndRewind"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc3e5acf-1194-46b2-a6ca-ab70ab3750b3"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""EndRewind"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d073472c-397a-4020-af2a-1f9f3156a45c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36d3e65c-b284-4ec8-9f12-e1470389606f"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""662f04ed-858b-43ee-97dc-40bc7f27d5ba"",
            ""actions"": [
                {
                    ""name"": ""Unpause"",
                    ""type"": ""Button"",
                    ""id"": ""2938e789-f18e-4651-a9f9-e5d9748bc73d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d497907a-1d27-4257-a51a-395d516b4fb8"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae79d780-8f61-4cc0-8f8b-e4b1586252c4"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
        m_Gameplay_LockOn = m_Gameplay.FindAction("LockOn", throwIfNotFound: true);
        m_Gameplay_RotateCamera = m_Gameplay.FindAction("RotateCamera", throwIfNotFound: true);
        m_Gameplay_StartBlock = m_Gameplay.FindAction("StartBlock", throwIfNotFound: true);
        m_Gameplay_EndBlock = m_Gameplay.FindAction("EndBlock", throwIfNotFound: true);
        m_Gameplay_Attack = m_Gameplay.FindAction("Attack", throwIfNotFound: true);
        m_Gameplay_Dodge = m_Gameplay.FindAction("Dodge", throwIfNotFound: true);
        m_Gameplay_InitRewind = m_Gameplay.FindAction("InitRewind", throwIfNotFound: true);
        m_Gameplay_ScrubForwardOLD = m_Gameplay.FindAction("ScrubForwardOLD", throwIfNotFound: true);
        m_Gameplay_ScrubBackwardOLD = m_Gameplay.FindAction("ScrubBackwardOLD", throwIfNotFound: true);
        m_Gameplay_ScrubOLD = m_Gameplay.FindAction("ScrubOLD", throwIfNotFound: true);
        m_Gameplay_ReleaseLockOn = m_Gameplay.FindAction("ReleaseLockOn", throwIfNotFound: true);
        m_Gameplay_ToggleLockLeft = m_Gameplay.FindAction("ToggleLockLeft", throwIfNotFound: true);
        m_Gameplay_ToggleLockRight = m_Gameplay.FindAction("ToggleLockRight", throwIfNotFound: true);
        m_Gameplay_StartHeavy = m_Gameplay.FindAction("StartHeavy", throwIfNotFound: true);
        m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
        m_Gameplay_SwordDraw = m_Gameplay.FindAction("SwordDraw", throwIfNotFound: true);
        m_Gameplay_Sprint = m_Gameplay.FindAction("Sprint", throwIfNotFound: true);
        // Rewind
        m_Rewind = asset.FindActionMap("Rewind", throwIfNotFound: true);
        m_Rewind_Scrub = m_Rewind.FindAction("Scrub", throwIfNotFound: true);
        m_Rewind_Pause = m_Rewind.FindAction("Pause", throwIfNotFound: true);
        m_Rewind_EndRewind = m_Rewind.FindAction("EndRewind", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Unpause = m_Menu.FindAction("Unpause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Movement;
    private readonly InputAction m_Gameplay_LockOn;
    private readonly InputAction m_Gameplay_RotateCamera;
    private readonly InputAction m_Gameplay_StartBlock;
    private readonly InputAction m_Gameplay_EndBlock;
    private readonly InputAction m_Gameplay_Attack;
    private readonly InputAction m_Gameplay_Dodge;
    private readonly InputAction m_Gameplay_InitRewind;
    private readonly InputAction m_Gameplay_ScrubForwardOLD;
    private readonly InputAction m_Gameplay_ScrubBackwardOLD;
    private readonly InputAction m_Gameplay_ScrubOLD;
    private readonly InputAction m_Gameplay_ReleaseLockOn;
    private readonly InputAction m_Gameplay_ToggleLockLeft;
    private readonly InputAction m_Gameplay_ToggleLockRight;
    private readonly InputAction m_Gameplay_StartHeavy;
    private readonly InputAction m_Gameplay_Pause;
    private readonly InputAction m_Gameplay_SwordDraw;
    private readonly InputAction m_Gameplay_Sprint;
    public struct GameplayActions
    {
        private @ControlMap m_Wrapper;
        public GameplayActions(@ControlMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputAction @LockOn => m_Wrapper.m_Gameplay_LockOn;
        public InputAction @RotateCamera => m_Wrapper.m_Gameplay_RotateCamera;
        public InputAction @StartBlock => m_Wrapper.m_Gameplay_StartBlock;
        public InputAction @EndBlock => m_Wrapper.m_Gameplay_EndBlock;
        public InputAction @Attack => m_Wrapper.m_Gameplay_Attack;
        public InputAction @Dodge => m_Wrapper.m_Gameplay_Dodge;
        public InputAction @InitRewind => m_Wrapper.m_Gameplay_InitRewind;
        public InputAction @ScrubForwardOLD => m_Wrapper.m_Gameplay_ScrubForwardOLD;
        public InputAction @ScrubBackwardOLD => m_Wrapper.m_Gameplay_ScrubBackwardOLD;
        public InputAction @ScrubOLD => m_Wrapper.m_Gameplay_ScrubOLD;
        public InputAction @ReleaseLockOn => m_Wrapper.m_Gameplay_ReleaseLockOn;
        public InputAction @ToggleLockLeft => m_Wrapper.m_Gameplay_ToggleLockLeft;
        public InputAction @ToggleLockRight => m_Wrapper.m_Gameplay_ToggleLockRight;
        public InputAction @StartHeavy => m_Wrapper.m_Gameplay_StartHeavy;
        public InputAction @Pause => m_Wrapper.m_Gameplay_Pause;
        public InputAction @SwordDraw => m_Wrapper.m_Gameplay_SwordDraw;
        public InputAction @Sprint => m_Wrapper.m_Gameplay_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @LockOn.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLockOn;
                @LockOn.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLockOn;
                @LockOn.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLockOn;
                @RotateCamera.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotateCamera;
                @StartBlock.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStartBlock;
                @StartBlock.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStartBlock;
                @StartBlock.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStartBlock;
                @EndBlock.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEndBlock;
                @EndBlock.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEndBlock;
                @EndBlock.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEndBlock;
                @Attack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Dodge.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodge;
                @Dodge.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodge;
                @Dodge.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodge;
                @InitRewind.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInitRewind;
                @InitRewind.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInitRewind;
                @InitRewind.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInitRewind;
                @ScrubForwardOLD.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScrubForwardOLD;
                @ScrubForwardOLD.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScrubForwardOLD;
                @ScrubForwardOLD.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScrubForwardOLD;
                @ScrubBackwardOLD.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScrubBackwardOLD;
                @ScrubBackwardOLD.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScrubBackwardOLD;
                @ScrubBackwardOLD.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScrubBackwardOLD;
                @ScrubOLD.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScrubOLD;
                @ScrubOLD.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScrubOLD;
                @ScrubOLD.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnScrubOLD;
                @ReleaseLockOn.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnReleaseLockOn;
                @ReleaseLockOn.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnReleaseLockOn;
                @ReleaseLockOn.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnReleaseLockOn;
                @ToggleLockLeft.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleLockLeft;
                @ToggleLockLeft.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleLockLeft;
                @ToggleLockLeft.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleLockLeft;
                @ToggleLockRight.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleLockRight;
                @ToggleLockRight.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleLockRight;
                @ToggleLockRight.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleLockRight;
                @StartHeavy.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStartHeavy;
                @StartHeavy.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStartHeavy;
                @StartHeavy.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStartHeavy;
                @Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @SwordDraw.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwordDraw;
                @SwordDraw.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwordDraw;
                @SwordDraw.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwordDraw;
                @Sprint.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @LockOn.started += instance.OnLockOn;
                @LockOn.performed += instance.OnLockOn;
                @LockOn.canceled += instance.OnLockOn;
                @RotateCamera.started += instance.OnRotateCamera;
                @RotateCamera.performed += instance.OnRotateCamera;
                @RotateCamera.canceled += instance.OnRotateCamera;
                @StartBlock.started += instance.OnStartBlock;
                @StartBlock.performed += instance.OnStartBlock;
                @StartBlock.canceled += instance.OnStartBlock;
                @EndBlock.started += instance.OnEndBlock;
                @EndBlock.performed += instance.OnEndBlock;
                @EndBlock.canceled += instance.OnEndBlock;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Dodge.started += instance.OnDodge;
                @Dodge.performed += instance.OnDodge;
                @Dodge.canceled += instance.OnDodge;
                @InitRewind.started += instance.OnInitRewind;
                @InitRewind.performed += instance.OnInitRewind;
                @InitRewind.canceled += instance.OnInitRewind;
                @ScrubForwardOLD.started += instance.OnScrubForwardOLD;
                @ScrubForwardOLD.performed += instance.OnScrubForwardOLD;
                @ScrubForwardOLD.canceled += instance.OnScrubForwardOLD;
                @ScrubBackwardOLD.started += instance.OnScrubBackwardOLD;
                @ScrubBackwardOLD.performed += instance.OnScrubBackwardOLD;
                @ScrubBackwardOLD.canceled += instance.OnScrubBackwardOLD;
                @ScrubOLD.started += instance.OnScrubOLD;
                @ScrubOLD.performed += instance.OnScrubOLD;
                @ScrubOLD.canceled += instance.OnScrubOLD;
                @ReleaseLockOn.started += instance.OnReleaseLockOn;
                @ReleaseLockOn.performed += instance.OnReleaseLockOn;
                @ReleaseLockOn.canceled += instance.OnReleaseLockOn;
                @ToggleLockLeft.started += instance.OnToggleLockLeft;
                @ToggleLockLeft.performed += instance.OnToggleLockLeft;
                @ToggleLockLeft.canceled += instance.OnToggleLockLeft;
                @ToggleLockRight.started += instance.OnToggleLockRight;
                @ToggleLockRight.performed += instance.OnToggleLockRight;
                @ToggleLockRight.canceled += instance.OnToggleLockRight;
                @StartHeavy.started += instance.OnStartHeavy;
                @StartHeavy.performed += instance.OnStartHeavy;
                @StartHeavy.canceled += instance.OnStartHeavy;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @SwordDraw.started += instance.OnSwordDraw;
                @SwordDraw.performed += instance.OnSwordDraw;
                @SwordDraw.canceled += instance.OnSwordDraw;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Rewind
    private readonly InputActionMap m_Rewind;
    private IRewindActions m_RewindActionsCallbackInterface;
    private readonly InputAction m_Rewind_Scrub;
    private readonly InputAction m_Rewind_Pause;
    private readonly InputAction m_Rewind_EndRewind;
    public struct RewindActions
    {
        private @ControlMap m_Wrapper;
        public RewindActions(@ControlMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Scrub => m_Wrapper.m_Rewind_Scrub;
        public InputAction @Pause => m_Wrapper.m_Rewind_Pause;
        public InputAction @EndRewind => m_Wrapper.m_Rewind_EndRewind;
        public InputActionMap Get() { return m_Wrapper.m_Rewind; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RewindActions set) { return set.Get(); }
        public void SetCallbacks(IRewindActions instance)
        {
            if (m_Wrapper.m_RewindActionsCallbackInterface != null)
            {
                @Scrub.started -= m_Wrapper.m_RewindActionsCallbackInterface.OnScrub;
                @Scrub.performed -= m_Wrapper.m_RewindActionsCallbackInterface.OnScrub;
                @Scrub.canceled -= m_Wrapper.m_RewindActionsCallbackInterface.OnScrub;
                @Pause.started -= m_Wrapper.m_RewindActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_RewindActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_RewindActionsCallbackInterface.OnPause;
                @EndRewind.started -= m_Wrapper.m_RewindActionsCallbackInterface.OnEndRewind;
                @EndRewind.performed -= m_Wrapper.m_RewindActionsCallbackInterface.OnEndRewind;
                @EndRewind.canceled -= m_Wrapper.m_RewindActionsCallbackInterface.OnEndRewind;
            }
            m_Wrapper.m_RewindActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Scrub.started += instance.OnScrub;
                @Scrub.performed += instance.OnScrub;
                @Scrub.canceled += instance.OnScrub;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @EndRewind.started += instance.OnEndRewind;
                @EndRewind.performed += instance.OnEndRewind;
                @EndRewind.canceled += instance.OnEndRewind;
            }
        }
    }
    public RewindActions @Rewind => new RewindActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Unpause;
    public struct MenuActions
    {
        private @ControlMap m_Wrapper;
        public MenuActions(@ControlMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Unpause => m_Wrapper.m_Menu_Unpause;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Unpause.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnUnpause;
                @Unpause.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnUnpause;
                @Unpause.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnUnpause;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Unpause.started += instance.OnUnpause;
                @Unpause.performed += instance.OnUnpause;
                @Unpause.canceled += instance.OnUnpause;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLockOn(InputAction.CallbackContext context);
        void OnRotateCamera(InputAction.CallbackContext context);
        void OnStartBlock(InputAction.CallbackContext context);
        void OnEndBlock(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnInitRewind(InputAction.CallbackContext context);
        void OnScrubForwardOLD(InputAction.CallbackContext context);
        void OnScrubBackwardOLD(InputAction.CallbackContext context);
        void OnScrubOLD(InputAction.CallbackContext context);
        void OnReleaseLockOn(InputAction.CallbackContext context);
        void OnToggleLockLeft(InputAction.CallbackContext context);
        void OnToggleLockRight(InputAction.CallbackContext context);
        void OnStartHeavy(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnSwordDraw(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface IRewindActions
    {
        void OnScrub(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnEndRewind(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnUnpause(InputAction.CallbackContext context);
    }
}
