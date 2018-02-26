//- PIE CHART -
var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
var pieChart = new Chart(pieChartCanvas)
var PieData = [
  {
      value: 700,
      color: '#f56954',
      highlight: '#f56954',
      label: 'Critical'
  },
  {
      value: 500,
      color: '#00a65a',
      highlight: '#00a65a',
      label: 'Minor'
  },
  {
      value: 400,
      color: '#f39c12',
      highlight: '#f39c12',
      label: 'Medium'
  }
]
var pieOptions = {
    segmentShowStroke: true,
    segmentStrokeColor: '#fff',
    segmentStrokeWidth: 2,
    percentageInnerCutout: 50,
    animationSteps: 100,
    animationEasing: 'easeOutBounce',
    animateRotate: true,
    animateScale: false,
    responsive: true,
    maintainAspectRatio: true,
    legendTemplate: '<ul class="<%=name.toLowerCase()%>-legend"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'
}
pieChart.Doughnut(PieData, pieOptions)