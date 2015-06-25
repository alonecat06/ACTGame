using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SimpleRpgCamera))]
public class SimpleRpgCameraEditor : Editor
{
	private string[] _toolbarChoices = new string[] { "Collision", "Target", "Movement", "Rotation", "Zoom", "Mobile" };
	private int _toolbarSelection = 0;

	private string[] _mobileChoices = new string[] { "Movement", "Rotation", "Zoom" };
	private int _mobileSelection = 0;

	private bool _foldInvert = false;
	private bool _foldObjectsToRotate = false;
	private bool _foldObjectsToFade = false;
	private bool _foldDistance = false;
	private bool _foldAngle = false;
	private bool _foldMouseAllow = false;
	private bool _foldMouseAllowObjectsToRotate = false;

	private int _objectsToRotateSize = 0;
	private int _objectsToFadeSize = 0;

	private bool _init = false;

	private GUIContent _content;

	private SimpleRpgCamera _self;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		_self = (SimpleRpgCamera)target;

		if(!_init)
		{
			_init = true;
			_objectsToRotateSize = _self.objectsToRotate.Count;
			_objectsToFadeSize = _self.objectsToFade.Count;
		}

		bool allowSceneObjects = !EditorUtility.IsPersistent(_self);

		_toolbarSelection = GUILayout.Toolbar(_toolbarSelection, _toolbarChoices);

		if(_toolbarSelection == 0)
		{
			#region Collision Settings

			if(_self.collisionLayers.value != 0)
			{
				_content = new GUIContent("Collision Buffer", "A small value to prevent camera clipping");
				_self.collisionBuffer = EditorGUILayout.FloatField(_content, _self.collisionBuffer);
			}

			if(_self.collisionAlphaLayers.value != 0)
			{
				_content = new GUIContent("Collision Alpha", "The alpha value for faded objects in front of the target");
				_self.collisionAlpha = EditorGUILayout.Slider(_content, _self.collisionAlpha, 0, 1);
				_content = new GUIContent("Collision Fade Speed", "Modifier for the time it takes to fade objects in / out");
				_self.collisionFadeSpeed = EditorGUILayout.FloatField(_content, _self.collisionFadeSpeed);
			}

			#endregion
		}
		else if(_toolbarSelection == 1)
		{
			#region Target Settings

			_content = new GUIContent("Target Tag", "Search for a target with the specified tag");
			_self.targetTag = EditorGUILayout.TextField(_content, _self.targetTag);
			_self.target = (Transform)EditorGUILayout.ObjectField("Target", _self.target, typeof(Transform), allowSceneObjects);
			_self.targetOffset = EditorGUILayout.Vector3Field("Target Offset", _self.targetOffset);
			_self.m_bSmoothOffset = EditorGUILayout.Toggle("Smooth Offset", _self.m_bSmoothOffset);

			if(_self.m_bSmoothOffset)
			{
				EditorGUI.indentLevel++;

				_self.smoothOffsetSpeed = EditorGUILayout.FloatField("Smooth Offset Speed", _self.smoothOffsetSpeed);

				EditorGUI.indentLevel--;
			}

			_self.m_bRelativeOffset = EditorGUILayout.Toggle("Relative Offset", _self.m_bRelativeOffset);
			_self.m_bUseTargetAxis = EditorGUILayout.Toggle("Use Target Axis", _self.m_bUseTargetAxis);

			#endregion
		}
		else if(_toolbarSelection == 2)
		{
			#region Movement Settings

			_self.allowMouseDrag = EditorGUILayout.Toggle("Allow Mouse Drag", _self.allowMouseDrag);

			if(_self.allowMouseDrag)
			{
				EditorGUI.indentLevel++;

				_self.mouseDragButton = (MouseButton)EditorGUILayout.EnumPopup("Drag Button", _self.mouseDragButton);
				_self.mouseDragSensitivity = EditorGUILayout.Vector2Field("Drag Sensitivity", _self.mouseDragSensitivity);

				EditorGUI.indentLevel--;
			}

			_self.allowEdgeMovement = EditorGUILayout.Toggle("Allow Edge Movement", _self.allowEdgeMovement);

			if(_self.allowEdgeMovement)
			{
				EditorGUI.indentLevel++;

				_self.edgePadding = EditorGUILayout.FloatField("Edge Padding", _self.edgePadding);

				_self.showEdges = EditorGUILayout.Toggle("Show Edges", _self.showEdges);

				if(_self.showEdges)
				{
					EditorGUI.indentLevel++;

					_self.edgeTexture = (Texture2D)EditorGUILayout.ObjectField("Edge Texture", _self.edgeTexture, typeof(Texture2D), allowSceneObjects);

					EditorGUI.indentLevel--;
				}

				EditorGUI.indentLevel--;
			}

			_self.allowEdgeKeys = EditorGUILayout.Toggle("Allow Keys", _self.allowEdgeKeys);

			if(_self.allowEdgeKeys)
			{
				EditorGUI.indentLevel++;

				_self.keyMoveUp = (KeyCode)EditorGUILayout.EnumPopup("Move Up Key", _self.keyMoveUp);
				_self.keyMoveDown = (KeyCode)EditorGUILayout.EnumPopup("Move Down Key", _self.keyMoveDown);
				_self.keyMoveLeft = (KeyCode)EditorGUILayout.EnumPopup("Move Left Key", _self.keyMoveLeft);
				_self.keyMoveRight = (KeyCode)EditorGUILayout.EnumPopup("Move Right Key", _self.keyMoveRight);

				EditorGUI.indentLevel--;
			}

			_self.lockToTarget = EditorGUILayout.Toggle("Lock To Target", _self.lockToTarget);

			_self.limitBounds = EditorGUILayout.Toggle("Limit Bounds", _self.limitBounds);

			if(_self.limitBounds)
			{
				EditorGUI.indentLevel++;

				_self.boundOrigin = EditorGUILayout.Vector3Field("Origin", _self.boundOrigin);
				_self.boundSize = EditorGUILayout.Vector3Field("Size", _self.boundSize);

				EditorGUI.indentLevel--;
			}

			_self.keyFollowTarget = (KeyCode)EditorGUILayout.EnumPopup("Follow Target Key", _self.keyFollowTarget);

			_self.scrollSpeed = EditorGUILayout.FloatField("Scroll Speed", _self.scrollSpeed);

			#endregion
		}
		else if(_toolbarSelection == 3)
		{
			#region Rotation Settings

			_self.originRotation = EditorGUILayout.Vector2Field("Origin Rotation", _self.originRotation);
			_self.stayBehindTarget = EditorGUILayout.Toggle("Stay Behind Target", _self.stayBehindTarget);
			_self.returnToOrigin = EditorGUILayout.Toggle("Return To Origin", _self.returnToOrigin);

			if(_self.returnToOrigin)
			{
				EditorGUI.indentLevel++;

				_self.setOriginLeft = EditorGUILayout.Toggle("Set With Left Button", _self.setOriginLeft);
				_self.setOriginMiddle = EditorGUILayout.Toggle("Set With Middle Button", _self.setOriginMiddle);
				_self.setOriginRight = EditorGUILayout.Toggle("Set With Right Button", _self.setOriginRight);

				EditorGUI.indentLevel--;
			}

			_self.allowRotation = EditorGUILayout.Toggle("Allow Rotation", _self.allowRotation);

			if(_self.allowRotation)
			{
				EditorGUI.indentLevel++;

				_self.mouseLook = EditorGUILayout.Toggle("Mouse Look", _self.mouseLook);

				if(_self.mouseLook)
				{
					EditorGUI.indentLevel++;
					_self.lockCursor = EditorGUILayout.Toggle("Lock Mouse", _self.lockCursor);
					_self.disableWhileUnlocked = EditorGUILayout.Toggle("Disable While Unlocked", _self.disableWhileUnlocked);

					_self.useJoystick = EditorGUILayout.Toggle("Use Joystick", _self.useJoystick);

					if(_self.useJoystick)
					{
						EditorGUI.indentLevel++;
						_self.joystickHorizontalAxis = EditorGUILayout.TextField("Joystick Horizontal Axis", _self.joystickHorizontalAxis);
						_self.joystickVerticalAxis = EditorGUILayout.TextField("Joystick Vertical Axis", _self.joystickVerticalAxis);
						EditorGUI.indentLevel--;
					}
					EditorGUI.indentLevel--;
				}
				else
				{
					_self.useJoystick = false;

					_foldMouseAllow = EditorGUILayout.Foldout(_foldMouseAllow, "Allowed Mouse Buttons");

					if(_foldMouseAllow)
					{
						EditorGUI.indentLevel++;

						_self.allowRotationLeft = EditorGUILayout.Toggle("Left Button", _self.allowRotationLeft);
						_self.allowRotationMiddle = EditorGUILayout.Toggle("Middle Button", _self.allowRotationMiddle);
						_self.allowRotationRight = EditorGUILayout.Toggle("Right Button", _self.allowRotationRight);

						EditorGUI.indentLevel--;
					}

					if(_self.allowRotationLeft || _self.allowRotationMiddle || _self.allowRotationRight)
					{
						_self.lockCursor = EditorGUILayout.Toggle("Lock Mouse", _self.lockCursor);

						if(_self.lockCursor)
						{
							EditorGUI.indentLevel++;

							if(_self.allowRotationLeft)
							{
								_self.lockLeft = EditorGUILayout.Toggle("Left Button", _self.lockLeft);
							}

							if(_self.allowRotationMiddle)
							{
								_self.lockMiddle = EditorGUILayout.Toggle("Middle Button", _self.lockMiddle);
							}

							if(_self.allowRotationRight)
							{
								_self.lockRight = EditorGUILayout.Toggle("Right Button", _self.lockRight);
							}

							EditorGUI.indentLevel--;
						}
					}
				}

				_foldAngle = EditorGUILayout.Foldout(_foldAngle, "Angle");

				if(_foldAngle)
				{
					EditorGUI.indentLevel++;
					_self.minAngle = EditorGUILayout.Slider("Min", _self.minAngle, -_self.maxAngle, _self.maxAngle);
					_self.maxAngle = EditorGUILayout.Slider("Max", _self.maxAngle, _self.minAngle, 85);
					EditorGUI.indentLevel--;
				}

				_foldInvert = EditorGUILayout.Foldout(_foldInvert, "Invert Rotation");

				if(_foldInvert)
				{
					EditorGUI.indentLevel++;
					_self.invertRotationX = EditorGUILayout.Toggle("X", _self.invertRotationX);
					_self.invertRotationY = EditorGUILayout.Toggle("Y", _self.invertRotationY);
					EditorGUI.indentLevel--;
				}

				_self.rotationSensitivity = EditorGUILayout.Vector2Field("Sensitivity", _self.rotationSensitivity);

				if(_self.allowRotationLeft || _self.allowRotationMiddle || _self.allowRotationRight)
				{
					_self.rotateObjects = EditorGUILayout.Toggle("Rotate Objects", _self.rotateObjects);

					if(_self.rotateObjects)
					{
						EditorGUI.indentLevel++;

						_self.autoAddTargetToRotate = EditorGUILayout.Toggle("Auto Add Target", _self.autoAddTargetToRotate);

						if(!_self.mouseLook)
						{
							_foldMouseAllowObjectsToRotate = EditorGUILayout.Foldout(_foldMouseAllowObjectsToRotate, "Allowed Mouse Buttons");

							if(_foldMouseAllowObjectsToRotate)
							{
								EditorGUI.indentLevel++;

								if(_self.allowRotationLeft)
								{
									_self.rotateObjectsLeft = EditorGUILayout.Toggle("Left Button", _self.rotateObjectsLeft);
								}

								if(_self.allowRotationMiddle)
								{
									_self.rotateObjectsMiddle = EditorGUILayout.Toggle("Middle Button", _self.rotateObjectsMiddle);
								}

								if(_self.allowRotationRight)
								{
									_self.rotateObjectsRight = EditorGUILayout.Toggle("Right Button", _self.rotateObjectsRight);
								}

								EditorGUI.indentLevel--;
							}
						}

						_foldObjectsToRotate = EditorGUILayout.Foldout(_foldObjectsToRotate, "Objects To Rotate");

						if(_foldObjectsToRotate)
						{
							EditorGUI.indentLevel++;

							_objectsToRotateSize = EditorGUILayout.IntField("Size", _objectsToRotateSize);

							if(_objectsToRotateSize < 0)
							{
								_objectsToRotateSize = 0;
							}

							Transform[] objectsToRotate = new Transform[_objectsToRotateSize];

							for(int i = 0; i < _objectsToRotateSize; i++)
							{
								if(_self.objectsToRotate.Count == i)
								{
									break;
								}

								objectsToRotate[i] = _self.objectsToRotate[i];
							}

							for(int i = 0; i < _objectsToRotateSize; i++)
							{
								objectsToRotate[i] = (Transform)EditorGUILayout.ObjectField("Element " + i, objectsToRotate[i], typeof(Transform), allowSceneObjects);
							}

							_self.objectsToRotate = new List<Transform>();

							foreach(Transform t in objectsToRotate)
							{
								if(t)
								{
									_self.objectsToRotate.Add(t);
								}
							}

							EditorGUI.indentLevel--;
						}

						EditorGUI.indentLevel--;
					}
				}

				EditorGUI.indentLevel--;
			}

			_self.allowRotationKeys = EditorGUILayout.Toggle("Allow Rotation Keys", _self.allowRotationKeys);

			if(_self.allowRotationKeys)
			{
				EditorGUI.indentLevel++;

				_self.keyRotateUp = (KeyCode)EditorGUILayout.EnumPopup("Up", _self.keyRotateUp);
				_self.keyRotateDown = (KeyCode)EditorGUILayout.EnumPopup("Down", _self.keyRotateDown);
				_self.keyRotateLeft = (KeyCode)EditorGUILayout.EnumPopup("Left", _self.keyRotateLeft);
				_self.keyRotateRight = (KeyCode)EditorGUILayout.EnumPopup("Right", _self.keyRotateRight);
				_self.rotationKeySensitivity = EditorGUILayout.Vector2Field("Sensitivity", _self.rotationKeySensitivity);

				EditorGUI.indentLevel--;
			}

			if(_self.allowRotation || _self.allowRotationKeys)
			{
				_self.rotationSmoothing = EditorGUILayout.FloatField("Smoothing", _self.rotationSmoothing);
			}

			#endregion
		}
		else if(_toolbarSelection == 4)
		{
			#region Zoom Settings

			_self.allowZoom = EditorGUILayout.Toggle("Allow Zoom", _self.allowZoom);

			_self.allowZoomKeys = EditorGUILayout.Toggle("Allow Zoom Keys", _self.allowZoomKeys);

			if(_self.allowZoomKeys)
			{
				EditorGUI.indentLevel++;

				_content = new GUIContent("Zoom In Key", "Key for zooming in");
				_self.keyZoomIn = (KeyCode)EditorGUILayout.EnumPopup(_content, _self.keyZoomIn);
				_content = new GUIContent("Zoom Out Key", "Key for zooming out");
				_self.keyZoomOut = (KeyCode)EditorGUILayout.EnumPopup(_content, _self.keyZoomOut);
				_content = new GUIContent("Zoom Key Delay", "The amount of time needed to hold the key down before constant zoom takes effect");
				_self.keyZoomDelay = EditorGUILayout.FloatField(_content, _self.keyZoomDelay);

				EditorGUI.indentLevel--;
			}

			if(_self.allowZoom || _self.allowZoomKeys)
			{
				EditorGUI.indentLevel++;

				_content = new GUIContent("Distance", "The distance between the camera and target");
				_foldDistance = EditorGUILayout.Foldout(_foldDistance, _content);

				if(_foldDistance)
				{
					EditorGUI.indentLevel++;

					_self.minDistance = EditorGUILayout.Slider("Min", _self.minDistance, 0.01f, _self.maxDistance);
					_self.distance = EditorGUILayout.Slider("Current", _self.distance, _self.minDistance, _self.maxDistance);
					_self.maxDistance = EditorGUILayout.Slider("Max", _self.maxDistance, _self.minDistance, 100);

					EditorGUI.indentLevel--;
				}

				_self.zoomSpeed = EditorGUILayout.FloatField("Zoom Speed", _self.zoomSpeed);
				_self.zoomSmoothing = EditorGUILayout.FloatField("Zoom Smoothing", _self.zoomSmoothing);
				_self.invertZoom = EditorGUILayout.Toggle("Invert Direction", _self.invertZoom);

				_self.fadeObjects = EditorGUILayout.Toggle("Fade Objects", _self.fadeObjects);

				if(_self.fadeObjects)
				{
					EditorGUI.indentLevel++;

					_self.autoAddTargetToFade = EditorGUILayout.Toggle("Auto Add Target", _self.autoAddTargetToFade);

					_self.fadeDistance = EditorGUILayout.FloatField("Fade Distance", _self.fadeDistance);

					_foldObjectsToFade = EditorGUILayout.Foldout(_foldObjectsToFade, "Objects To Fade");

					if(_foldObjectsToFade)
					{
						EditorGUI.indentLevel++;

						_objectsToFadeSize = EditorGUILayout.IntField("Size", _objectsToFadeSize);

						if(_objectsToFadeSize < 0)
						{
							_objectsToFadeSize = 0;
						}

						Renderer[] objectsToFade = new Renderer[_objectsToFadeSize];

						for(int i = 0; i < _objectsToFadeSize; i++)
						{
							if(_self.objectsToFade.Count == i)
							{
								break;
							}

							objectsToFade[i] = _self.objectsToFade[i];
						}

						for(int i = 0; i < _objectsToFadeSize; i++)
						{
							objectsToFade[i] = (Renderer)EditorGUILayout.ObjectField("Element " + i, objectsToFade[i], typeof(Renderer), allowSceneObjects);
						}

						_self.objectsToFade = new List<Renderer>();

						foreach(Renderer r in objectsToFade)
						{
							if(r)
							{
								_self.objectsToFade.Add(r);
							}
						}

						EditorGUI.indentLevel--;
					}

					EditorGUI.indentLevel--;
				}

				EditorGUI.indentLevel--;
			}

			#endregion
		}
		else if(_toolbarSelection == 5)
		{
			#region Mobile Settings

			EditorGUILayout.HelpBox("Note: Most of the desktop settings affect both desktop and mobile functionality. Settings in this tab that are highlighted in yellow will override the desktop settings on mobile devices for convenience.", MessageType.Info);

			_self.allowTouch = EditorGUILayout.Toggle("Allow Touch", _self.allowTouch);

			if(_self.allowTouch)
			{
				_self.touchSensitivity = EditorGUILayout.FloatField("Touch Sensitivity", _self.touchSensitivity);

				_mobileSelection = GUILayout.Toolbar(_mobileSelection, _mobileChoices);

				if(_mobileSelection == 0)
				{
					#region Mobile Movement Settings

					if(_self.allowEdgeMovement)
					{
						_self.mobilePanType = (SimpleRpgCamera.PanControlType)EditorGUILayout.EnumPopup("Movement Control Method", _self.mobilePanType);

						if(_self.mobilePanType == SimpleRpgCamera.PanControlType.Drag)
						{
							EditorGUI.indentLevel++;
							_self.mobilePanningTouchCount = EditorGUILayout.IntField("Panning Touch Count", _self.mobilePanningTouchCount);
							EditorGUI.indentLevel--;
						}
						else if(_self.mobilePanType == SimpleRpgCamera.PanControlType.Swipe)
						{
							EditorGUI.indentLevel++;
							_self.mobilePanSwipeActiveTime = EditorGUILayout.FloatField("Swipe Detection Time", _self.mobilePanSwipeActiveTime);
							_self.mobilePanSwipeMinDistance = EditorGUILayout.FloatField("Min Swipe Distance", _self.mobilePanSwipeMinDistance);
							_self.mobilePanSwipeDistance = EditorGUILayout.Vector2Field("Movement Distance", _self.mobilePanSwipeDistance);
							EditorGUI.indentLevel--;
						}
					}
					else
					{
						EditorGUILayout.HelpBox("Edge movement is disabled. Enable it in the Movement tab.", MessageType.Info);
					}

					#endregion
				}
				else if(_mobileSelection == 1)
				{
					#region Mobile Rotation Settings

					if(_self.allowRotation)
					{
						_self.mobileRotationType = (SimpleRpgCamera.RotationControlType)EditorGUILayout.EnumPopup("Rotation Control Method", _self.mobileRotationType);

						if(_self.mobileRotationType == SimpleRpgCamera.RotationControlType.Swipe)
						{
							EditorGUI.indentLevel++;
							_self.mobileSwipeActiveTime = EditorGUILayout.FloatField("Swipe Detection Time", _self.mobileSwipeActiveTime);
							_self.mobileSwipeMinDistance = EditorGUILayout.FloatField("Min Swipe Distance", _self.mobileSwipeMinDistance);
							_self.mobileSwipeRotationAmount = EditorGUILayout.Vector2Field("Swipe Rotation Amount", _self.mobileSwipeRotationAmount);
							EditorGUI.indentLevel--;
						}
						else if(_self.mobileRotationType == SimpleRpgCamera.RotationControlType.Drag)
						{
							EditorGUI.indentLevel++;
							_self.mobileRotationDelay = EditorGUILayout.FloatField("Rotation Delay", _self.mobileRotationDelay);
							EditorGUI.indentLevel--;
						}
					}
					else
					{
						EditorGUILayout.HelpBox("Rotation is disabled. Enable it in the Rotation tab.", MessageType.Info);
					}

					#endregion
				}
				else if(_mobileSelection == 2)
				{
					#region Mobile Zoom Settings

					if(_self.allowZoom)
					{
						_self.mobileZoomDeadzone = EditorGUILayout.FloatField("Zoom Deadzone", _self.mobileZoomDeadzone);
						GUI.color = Color.yellow;
						_self.mobileZoomSpeed = EditorGUILayout.FloatField("Zoom Speed", _self.mobileZoomSpeed);
						GUI.color = Color.white;
					}
					else
					{
						EditorGUILayout.HelpBox("Zoom is disabled. Enable it in the Zoom tab.", MessageType.Info);
					}

					#endregion
				}
			}

			#endregion
		}

		if(GUI.changed)
		{
			EditorUtility.SetDirty(_self);
		}
	}
}