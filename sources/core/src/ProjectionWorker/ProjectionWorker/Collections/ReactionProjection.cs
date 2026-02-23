using ProjectionWorker.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionWorker.Collections;
public class ReactionProjection : Document
{
    public string Name { get; set; }
    public string Icon { get; set; }

}
