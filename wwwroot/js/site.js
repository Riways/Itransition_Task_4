$("#mainSwitcher").click(function(){
    $('input:checkbox').not(this).prop('checked', this.checked);
});

$(".secondarySwitcher").click(function(){
    if(!this.checked && $("#mainSwitcher").is(":checked")){
        $("#mainSwitcher").prop('checked', false)
    }
    
})