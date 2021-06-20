const url = ["Parser/GetCode", "Parser/GetDisease", "Search/GetServices", "Search/GetMedicines", "Search/GetMkb", "Standart/GetStandartMkb", "Standart/GetStandartServices", "Standart/GetStandartMedicament"];
const sex = ["любой", "мужчины", "женщины"];
const age = ["любой", "дети", "взрослые"];
function update_result(html) {
    $("#result").html(html);
    if ($("#result").find("table").length > 0) $("#result").find("table").addClass("table table-hover table-sm");
    $("#start, #error").hide();
    $("#result").show();
};
function update_error() {
    $("#start,#result").hide();
    $("#error").show();
}
function Parser(id, data) {
    let hr = id == 0 ? "Найденные коды МКБ-10" : "Найденные заболевания";
    var html = "<table><thead><tr><th>" + hr + "</th></tr></thead><tbody>";
    for (var i = 0; i < data.search.length; i++) html += "<tr><td>" + data.search[i] + "</td></tr>";
    html += "</tbody></table>";
    update_result(html);
}
function SearchMedicines(data) {
    var html = "<table><thead><tr><th>Стандартизированное наименование</th><th>Торговое наименование</th></tr></thead><tbody>";
    for (var i = 0; i < data.items.length; i++) html += "<tr><td>" + data.items[i].standart + "</td><td>" + data.items[i].commercial + "</td></tr>";
    html += "</tbody></table>";
    update_result(html);
}
function SearchServices(data) {
    var html = "<table><thead><tr><th>Код</th><th>Наименование</th><th>Вероятность</th></tr></thead><tbody>";
    for (var i = 0; i < data.items[0].items.length; i++) html += "<tr><td>" + data.items[0].items[i].code + "</td><td>" + data.items[0].items[i].name + "</td><td>" + data.items[0].items[i].weight + "</td></tr>";
    html += "</tbody></table>";
    update_result(html);
}
function SearchMkb(data) {
    var html = "<table><caption>По найденным кодам мкб-10: </caption><thead><tr><th>Код</th><th>Наименование</th></tr></thead><tbody>";
    for (var i = 0; i < data.items[0].code.length; i++) html += "<tr><td>" + data.items[0].code[i].code + "</td><td>" + data.items[0].code[i].name + "</td></tr>";
    html += "</tbody></table><table class='mt-4'><caption>По найденным заболеваниям: </caption><thead><tr><th>Код</th><th>Наименование</th><th>Вероятность</th></tr></thead><tbody>";
    for (var i = 0; i < data.items[0].items.length; i++) html += "<tr><td>" + data.items[0].items[i].code + "</td><td>" + data.items[0].items[i].name + "</td><td>" + data.items[0].items[i].weight + "</td></tr>";
    html += "</tbody></table>";
    update_result(html);
}
function Standart(data) {
    var html = "";
    for (var i = 0; i < data.items.length; i++) {
        html += '<input class="hide" id="standart-' + i + '" type="checkbox"><label for="standart-' + i + '">' + data.items[i].name + '</label><div>';
        html += "<div class='row mb-2 text-muted'><div class='col-auto'><b>ПОЛ:</b> " + sex[data.items[i].sex] + "</div class='col-auto'><div><b>ВОЗРАСТ:</b> " + age[data.items[i].age] + "</div></div>";
        html += "<div class='mt-2'><b>Рекомендации</b> " + data.items[i].recommendations.brief_info+"</div>";
        html += "<div class='ui-info mt-2'><div class='position-sticky bg-white top-0'><b class='text-muted'>Оказываемые услуги</b></div><table class='t-9'><thead><tr><th>Код</th><th>Наименование</th><th>Усредненный показатель частоты предоставления</th><th>Усредненный показатель кратности применения</th></tr></thead><tbody>";
        for (var j = 0; j < data.items[i].services.length; j++) {
            html += "<tr><td>" + data.items[i].services[j].code + "</td><td>" + data.items[i].services[j].name + "</td><td>" + data.items[i].services[j].weight + "</td><td>" + data.items[i].services[j].count + "</td></tr>";
        }
        html += "</tbody></table></div>";
        html += "<div class='ui-info mt-2'><div class='position-sticky bg-white top-0'><b class='text-muted'>Назначаемые лекарства</b></div><table class='t-9'><thead><tr><th>Код</th><th>Наименование</th><th>Усредненный показатель частоты предоставления</th></tr></thead><tbody>";
        for (var j = 0; j < data.items[i].medicament.length; j++) {
            html += "<tr><td>" + data.items[i].medicament[j].code + "</td><td>" + data.items[i].medicament[j].name + "</td><td>" + data.items[i].medicament[j].weight + "</td></tr>";
        }
        html += "</tbody></table><div>";
        html += "</div>";
    }
    html += "";
    update_result(html);
}
$(document).ready(function () {
    $(".save").click(function () {
        let id = isNaN(parseInt($('#select').val())) ? null : parseInt($('#select').val()),
            sexv = isNaN(parseInt($('#select-sex').val())) ? 0 : parseInt($('#select-sex').val()),
            agev = isNaN(parseInt($('#select-age').val())) ? 0 : parseInt($('#select-age').val());
        let value = $("#input").val().trim();
        if (id != null && id >= 0 && id < url.length && value.length > 0) {
            var but = $(this); but.prop('disabled', true); but.find("span").fadeIn();
            $.get("/api/" + url[id], { data: value, sex: sexv, age: agev }).always(() => { but.prop('disabled', false); but.find("span").fadeOut(); })
                .done(data => {
                    console.log(data);
                    console.log(data.code);
                    console.log(id);
                    if (data.code == 200) {
                        switch (id) {
                            case 0:
                            case 1: Parser(id, data); break;
                            case 2: SearchServices(data); break;
                            case 3: SearchMedicines(data); break;
                            case 4: SearchMkb(data); break;
                            case 5:
                            case 6:
                            case 7: Standart(data);
                                break;
                        }
                    }
                    else update_error();
                }).fail(() => update_error());
        }
    });
    $('#select').on('change', function (e) {
        let id = isNaN(parseInt($('#select').val())) ? null : parseInt($('#select').val());
        console.log(id);
        if (id != null && id >= 5 && id <= 7) $("#search-standart").show();
        else $("#search-standart").hide();
    });
});
var data = ["aaaa", "aaaa"];
$.get("/api/Search/GetMkbSet/", {data: data}).done(data => { console.log(data);});