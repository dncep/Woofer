using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Camera
{
    [ComponentSystem("camera_region_system", ProcessingCycles.None),
        Watching(typeof(CameraRegionComponent)),
        Listening(typeof(CameraLocationQueryEvent))]
    class CameraRegionSystem : ComponentSystem
    {
        public override void EventFired(object sender, Event evt)
        {
            if(evt is CameraLocationQueryEvent qe)
            {
                foreach(CameraRegionComponent region in WatchedComponents)
                {
                    Spatial sp = region.Owner.Components.Get<Spatial>();
                    Rectangle area = region.Area;
                    if (sp != null) area += sp.Position;
                    if(area.Contains(qe.SuggestedLocation))
                    {
                        region.Easing += 0.01;
                        if (region.Easing > 1) region.Easing = 1;

                        Vector2D oldPos = qe.SuggestedLocation;
                        Vector2D newPos = region.Focus;

                        if (sp != null) newPos += sp.Position;

                        Vector2D mixedPos = region.Easing * newPos + (1 - region.Easing) * oldPos;

                        qe.SuggestedLocation = mixedPos;
                    } else if(region.Easing > 0)
                    {
                        region.Easing -= 0.01;
                        if (region.Easing < 0) region.Easing = 0;

                        Vector2D oldPos = qe.SuggestedLocation;
                        Vector2D newPos = region.Focus;

                        if (sp != null) newPos += sp.Position;

                        Vector2D mixedPos = region.Easing * newPos + (1 - region.Easing) * oldPos;

                        qe.SuggestedLocation = mixedPos;
                    }
                }
            }
        }
    }
}
