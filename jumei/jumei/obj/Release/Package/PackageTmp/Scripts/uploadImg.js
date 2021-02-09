$(function () {
    var $proImages = $("textarea[name='product[images]']");
    var $spuImages = $(".spu_images");
    var $spuVerticalimages = $(".spu_vertical_images");

    //spu 图
    $spuImages.each(function () {
        var _this = $(this);
        if (_this.text() == "") {
            var str = '[{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0}]';
            _this.text(str);
        } else {
            try {
                var imagesjson = JSON.parse(_this.val());
                var pics = _this.next().find(".pic-spu");
                $.each(imagesjson, function (i, item) {
                    if (item.img != "") {
                        $(pics[i]).find(".imgWrap").addClass("on").find("img").attr("src", item.img);
                        $(pics[i]).find(".ico-look").attr("href", item.img);
                        $(pics[i]).find(".upWord").addClass("hide");
                    }
                });
            } catch (e) {
                console.log("error log:子型号白底方图格式出错");
            }
        }
    });
    //spu 竖图
    $spuVerticalimages.each(function () {
        var _this = $(this);
        if (_this.text() == "") {
            var str = '{"vertical_images":[{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0}]}';
            _this.text(str);
        } else {
            try {
                var imagesjson = JSON.parse(_this.val());
                var pics = _this.next().find(".pic-spu-vertical");
                $.each(imagesjson.vertical_images, function (i, item) {
                    if (item.img != "") {
                        $(pics[i]).find(".imgWrap").addClass("on").find("img").attr("src", item.img);
                        $(pics[i]).find(".ico-look").attr("href", item.img);
                        $(pics[i]).find(".upWord").addClass("hide");
                    }
                });
            } catch (e) {
                console.log("error log:子型号竖图格式出错");
            }
        }
    });

    //产品主图&调性图
    if ($proImages.text() == "") {
        var str = '{"normal":[{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0}],"vertical":[{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0},{"img":"","is_changed":0}],"diaoxing":{"img":"","is_changed":0}}';
        $proImages.text(str);
    } else {
        try {
            var imagesjson = JSON.parse($proImages.val());
            var pics = $(".pic-normal");
            $.each(imagesjson.normal, function (i, item) {
                if (item.img != "") {
                    $(pics[i]).find(".imgWrap").addClass("on").find("img").attr("src", item.img);
                    $(pics[i]).find(".ico-look").attr("href", item.img);
                    $(pics[i]).find(".upWord").addClass("hide");
                }
            });
            var picvertical = $(".pic-vertical");
            $.each(imagesjson.vertical, function (i, item) {
                if (item.img != "") {
                    $(picvertical[i]).find(".imgWrap").addClass("on").find("img").attr("src", item.img);
                    $(picvertical[i]).find(".ico-look").attr("href", item.img);
                    $(picvertical[i]).find(".upWord").addClass("hide");
                }
            });
            var $pic_diaox = $(".tonality");
            var diaox_img = imagesjson.diaoxing.img;
            if (diaox_img != "") {
                $pic_diaox.find(".imgWrap").addClass("on").find("img").attr("src", diaox_img);
                $pic_diaox.find(".ico-look").attr("href", diaox_img);
                $pic_diaox.find(".upWord").addClass("hide");
            }
        } catch (e) {
            console.log("error log:产品图片格式出错");
        }
    }
    $('.edit-cont').off('click').delegate('.pic-input', 'change', function () {
        var $this = $(this),
            $parent = $this.parent(),
            $target_p = $parent.find(".imgWrap"),
            $target = $parent.find(".imgWrap img"),
            $imageloadstatus = $parent.find(".imageloadstatus"),
            $tip = $parent.find(".upWord"),
            $ertip = $this.closest(".msgWrap").find(".er"),
            $ok = $this.closest(".msgWrap").find(".ok"),
            $look = $this.closest(".pic-holder").find(".ico-look"),
            sort = parseInt($this.closest(".pic-holder").attr("data-sort")) + 1,
            isNormal = $this.closest('.pic-holder').hasClass("pic-normal"),
            isVertical = $this.closest('.pic-holder').hasClass("pic-vertical"),
            isDiaoxing = $this.closest('.pic-holder').hasClass("tonality");
        isSpuVertical = $this.closest('.pic-holder').hasClass("pic-spu-vertical");
        isSpu = $this.closest('.pic-holder').hasClass("pic-spu");

        $this.attr("form", "fileForm");
        var form = $("#fileForm"), type = form.find("input[name=type]"), $textarea;
        if (isNormal) {
            type.val("normal");
            $textarea = $proImages;
        } else if (isVertical) {
            type.val("vertical");
            $textarea = $proImages;
        }
        else if (isDiaoxing) {
            type.val("diaoxing");
            $textarea = $proImages;
        } else if (isSpuVertical) {
            type.val("vertical");
            $textarea = $this.closest('td').find(".spu_vertical_images");
        } else if (isSpu) {
            type.val("normal");
            $textarea = $this.closest('td').find(".spu_images");
        }
        if (this.value) {
            form.ajaxForm({
                target: $target,
                beforeSubmit: function () {
                    $imageloadstatus.show();
                    $tip.hide();
                },
                success: function (data) {
                    $this.val("").attr("form", "fileFormTmp");
                    if (data.error) {
                        $tip.show();
                        $imageloadstatus.hide();
                        $ertip.html(data.message).show();
                        setTimeout(function () {
                            $ertip.fadeOut();
                        }, 2000);
                    } else {
                        $imageloadstatus.hide();
                        $target.attr("src", data.message + "?" + Math.round(545784 * Math.random()));
                        $look.attr("href", data.message + "?" + Math.round(545784 * Math.random()));
                        $tip.hide();
                        $target_p.addClass("on");
                        var imagesjson = JSON.parse($textarea.val());
                        if (isDiaoxing) {
                            imagesjson.diaoxing.is_changed = 1;
                            imagesjson.diaoxing.img = data.message;
                            $textarea.text(JSON.stringify(imagesjson));
                        } else if (isNormal) {
                            $ok.html("图 " + sort + "：上传成功！").show();
                            setTimeout(function () {
                                $ok.fadeOut();
                            }, 2000);
                            $.each(imagesjson.normal, function (i, item) {
                                if (i + 1 == sort) {
                                    item.is_changed = 1;
                                    item.img = data.message;
                                }
                            });
                            $textarea.text(JSON.stringify(imagesjson));
                        } else if (isVertical) {
                            $ok.html("图 " + sort + "：上传成功！").show();
                            setTimeout(function () {
                                $ok.fadeOut();
                            }, 2000);
                            $.each(imagesjson.vertical, function (i, item) {
                                if (i + 1 == sort) {
                                    item.is_changed = 1;
                                    item.img = data.message;
                                }
                            });
                            $textarea.text(JSON.stringify(imagesjson));
                        } else if (isSpuVertical) {
                            $ok.html("图 " + sort + "：上传成功！").show();
                            setTimeout(function () {
                                $ok.fadeOut();
                            }, 2000);
                            $.each(imagesjson.vertical_images, function (i, item) {
                                if (i + 1 == sort) {
                                    item.is_changed = 1;
                                    item.img = data.message;
                                }
                            });
                            $textarea.text(JSON.stringify(imagesjson));
                        } else if (isSpu) {
                            $ok.html("图 " + sort + "：上传成功！").show();
                            setTimeout(function () {
                                $ok.fadeOut();
                            }, 2000);
                            $.each(imagesjson, function (i, item) {
                                if (i + 1 == sort) {
                                    item.is_changed = 1;
                                    item.img = data.message;
                                }
                            });
                            $textarea.text(JSON.stringify(imagesjson));
                        }
                    }
                },
                error: function (data) {
                    $imageloadstatus.hide();
                    $tip.show();
                    $ertip.html("图 " + sort + "：上传失败，可能是网络原因！").show();
                    setTimeout(function () {
                        $ertip.fadeOut();
                    }, 2000);
                    $this.val("").attr("form", "fileFormTmp");
                }
            }).submit();
        }
    });

    $("#proEdit").delegate(".pic-holder", "mouseenter", function () {
        if ($(this).find(".imgWrap").hasClass("on")) {
            $(this).find(".oprate").show();
        }
    }).delegate(".pic-holder", "mouseleave", function () {
        if ($(this).find(".imgWrap").hasClass("on")) {
            $(this).find(".oprate").hide();
        }
    });
});