using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RealtimeModel]
public class PlayerSyncModel
{
    [RealtimeProperty(1, true, true)] private Color _color;
}
