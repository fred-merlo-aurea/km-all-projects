function renderVenn(divID, sets) {
    try {
        var chart = venn.VennDiagram()
						 .width(400)
						 .height(300);


        var div = d3.select(divID)

        div.datum(sets).call(chart);

        var tooltip = d3.select("body").append("div").attr("class", "venntooltip");

        // div.select("path")
        //	.style("stroke-opacity", 0)
        //	.style("stroke", "#fff")
        //	.style("stroke-width", 0)

        div.selectAll("g")
			.on("mouseover", function (d, i) {
			    // sort all the areas relative to the current item
			    venn.sortAreas(div, d);

			    // Display a tooltip with the current size
			    tooltip.transition().duration(400).style("opacity", .9);
			    tooltip.text(d.size + " users");

			    // highlight the current path
			    var selection = d3.select(this).transition("tooltip").duration(400);
			    selection.select("path")
					.style("stroke-width", 3)
					.style("stroke", "#fff")
					.style("fill-opacity", d.sets.length == 1 ? .4 : .1)
					.style("stroke-opacity", 1);
			})

			.on("mousemove", function () {
			    tooltip.style("left", (d3.event.pageX) + "px")
					   .style("top", (d3.event.pageY - 28) + "px");
			})

			.on("mouseout", function (d, i) {
			    tooltip.transition().duration(400).style("opacity", 0);
			    var selection = d3.select(this).transition("tooltip").duration(400);
			    selection.select("path")
					.style("stroke-width", 0)
					.style("fill-opacity", d.sets.length == 1 ? .25 : .0)
					.style("stroke-opacity", 0);
			});
    }
    catch (err) {
        //alert("Error Rendering Venn :" + err);
    }
}