$(function () {
    //模拟城市-无联动/无搜索
    var selector = $('#city-picker-selector').cityPicker({
        dataJson: cityData,
        renderMode: true,
        search: false,
        linkage: false
    })
    $('#city-picker-selector').on('choose-province.citypicker', function (event, tagert, storage) {
        console.log(storage);
    });

    //模拟城市-联动/搜索
    $('#city-picker-search').cityPicker({
        dataJson: cityData,
        renderMode: true,
        search: true,
        linkage: true
    });
});